#!/usr/bin/env python
#
# Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Runs sample testing on solarbeam vs reference code, yielding mean, max and
# standard deviation.

import math
import os
import os.path
import random
import re
import subprocess
import string
import sys
import time

re_az = "(?s)azimuth\s*:\s*([0-9-.]+)"
re_el = "(?s)elevation\s*:\s*([0-9-.]+)"
re_rise = "(?s)sunrise\s*:\s*([^\s]+)"
re_noon = "(?s)noon\s*:\s*([^\s]+)"
re_set = "(?s)sunset\s*:\s*([^\s]+)"

def p(s):
    sys.stdout.write(s + "\n")
    sys.stdout.flush()

def invoke(cwd, args):
    popen = subprocess.Popen(args, cwd=cwd,
                 stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    (out, _err) = popen.communicate()
    out = str(out).strip()
    return popen.returncode, out

class ParseException(Exception): pass

rx = "([0-9]+):([0-9]+):([0-9]+)"
def parsetime(s):
    m = re.search(rx, s)
    hour, min, sec = m.group(1), m.group(2), m.group(3)
    return int(hour) * 3600 + int(min) * 60 + int(sec)

def parse(rx, out):
    m = re.search(rx, out)
    if m:
        v = m.group(1)
        if v == "none":
            return 0
        try:
            v = float(v)
            return v
        except:
            try:
                return parsetime(v)
            except:
                raise ParseException, "Failed to parse: %s" % out
    raise ParseException, "Failed to parse: %s" % out

def parse_all(out):
    az = parse(re_az, out)
    el = parse(re_el, out)
    rise = parse(re_rise, out)
    noon = parse(re_noon, out)
    set = parse(re_set, out)
    return az, el, rise, noon, set

def run_srrb(args):
    args = ["smjs", "srrb_el.js"]
    args.extend(map(str, js))
    (_, out) = invoke(".", args)
    args = ["smjs", "srrb_sns.js"]
    args.extend(map(str, js))
    (_, out2) = invoke(".", args)
    out = out + "\n\n" + out2
    return out, parse_all(out)

def run_solarbeam(args):
    args = ["./cli"]
    args.extend(map(str, lu))
    (_, out) = invoke("..", args)
    return out, parse_all(out)

def diff_float(x, y):
    if x == 0 or y == 0:
        return 0
    diff = abs(x - y)
    if diff >= 43200: # 24 rollover bug - trigger at half day
        diff = abs(86400 - diff)
    return diff
    

class Sequence(object):
    def __init__(self):
        self.n = 0
        self.max = 0
        self.sum = 0
        self.sumsq = 0
    def add(self, v):
        self.n += 1
        self.sum += v
        self.sumsq += v * v
        if v > self.max:
            self.max = v
            return True
    def avg(self):
        return self.sum / float(self.n)
    def stddev(self):
        avg = self.avg()
        return math.sqrt(self.sumsq / float(self.n) - avg * avg)

sum = 360 * 60 * 60 * 60 * 180 * 60 * 60 * 20 * 12 * 30 * 24 * 60 * 60

col = {}
for e in ("azimuth", "elevation", "sunrise", "noon", "sunset"):
    col[e] = Sequence()

def print_status(col, vals):
    print
    for k in vals.keys():
        print "%10.10s   max %12.7g   avg %12.7g   sig %12.7g" %\
                (k, col[k].max, col[k].avg(), col[k].stddev())
    print


it = 1
while True:
    try:
        lodeg = random.randint(0, 179)
        lomin = random.randint(0, 59);
        losec = random.randint(0, 59);
        lodir = random.randint(0, 1) == 0 and "E" or "W"

        ladeg = random.randint(0, 89);
        lamin = random.randint(0, 59);
        lasec = random.randint(0, 59);
        ladir = random.randint(0, 1) == 0 and "N" or "S"

        tz = lodeg / 15

        year = random.randint(1990, 2010);
        month = random.randint(1, 12);
        day = random.randint(1, 30);
        if month == 2:
            day = random.randint(1, 28);
        hour = random.randint(0, 23);
        min = random.randint(0, 59);
        sec = random.randint(0, 59);

        print ("%6.6s  %1.1s %3.3s %2.2s %2.2s  %1.1s %2.2s %2.2s %2.2s  %2.2s:%2.2s:%2.2s  %2.2s.%2.2s.%4.4s" %
               (it, 
                lodir, lodeg, lomin, losec, 
                ladir, ladeg, lamin, lasec,
                string.zfill(hour, 2),
                string.zfill(min, 2),
                string.zfill(sec, 2),
                string.zfill(day, 2),
                string.zfill(month, 2),
                year))

        js = [lodir, lodeg, lomin, losec,
              ladir, ladeg, lamin, lasec,
              tz,
              day, month, year,
              hour, min, sec]
        lu = ["--lon", ("%s.%s.%s.%s" % (lodir, lodeg, lomin, losec)),
              "--lat",  ("%s.%s.%s.%s" % (ladir, ladeg, lamin, lasec)),
              "--dt",      ("%s.%s.%s" % (day, month, year)),
              "--tm",      ("%s:%s:%s" % (hour, min, sec)),
              "--tz",  tz]

        o, o2 = "", ""
        try:
            o, (saz, sel, srise, snoon, sset) = run_srrb(js)
        except ParseException:
            sys.stderr.write("js parse fail: %s\n" % reduce(lambda x, y: "%s %s" % (x, y), js))
            continue

        try:
            o2, (laz, lel, lrise, lnoon, lset) = run_solarbeam(lu)
        except ParseException:
            sys.stderr.write("lu parse fail: %s\n" % reduce(lambda x, y: "%s %s" % (x, y), lu))
            continue

        vals = {}
        vals["azimuth"] = diff_float(saz, laz)
        vals["elevation"] = diff_float(sel, lel)
        vals["sunrise"] = diff_float(srise, lrise)
        vals["noon"] = diff_float(snoon, lnoon)
        vals["sunset"] = diff_float(sset, lset)

        for k in vals.keys():
            is_max = col[k].add(vals[k])
            if is_max:

                s = "================ %s %s ================" % (k, vals[k])
                sys.stderr.write("%s\n%s\n\n\n%s\n\n\n" % (s, o, o2))

        if it % 50 == 0:
            print_status(col, vals)

    except KeyboardInterrupt:
        print_status(col, vals)
        sys.exit()

    it += 1
