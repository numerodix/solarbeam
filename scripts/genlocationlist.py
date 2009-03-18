#!/usr/bin/env python
# -*- coding: UTF-8 -*-
#
# Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Generate c# code for LocationList using location data file from
# http://www.world-gazetteer.com.

import string
import sys


class Location(object):
    def __init__(self, id, name, aliases, cat, pop, lat, lon, country, region):
        self.id = id
        self.name = name
        self.aliases = aliases
        self.cat = cat
        self.pop = pop
        self.lat = lat
        self.lon = lon
        self.country = country
        self.region = region
    def __str__(self):
        s = "%s\t%s\t%s\t%s" % (self.id, self.getname(), self.pop, self.country)
        return s
    def getname(self):
        name = self.name

        # simplify input
        uname = unicode(name, "utf-8")
        uname = string.replace(uname, unicode("ā", "utf-8"), "a")
        uname = string.replace(uname, unicode("Ā", "utf-8"), "A")
        uname = string.replace(uname, unicode("á", "utf-8"), "a")
        uname = string.replace(uname, unicode("ã", "utf-8"), "a")
        uname = string.replace(uname, unicode("à", "utf-8"), "a")
        uname = string.replace(uname, unicode("â", "utf-8"), "a")
        uname = string.replace(uname, unicode("ả", "utf-8"), "a")
        uname = string.replace(uname, unicode("ă", "utf-8"), "a")
        uname = string.replace(uname, unicode("ç", "utf-8"), "c")
        uname = string.replace(uname, unicode("ě", "utf-8"), "e")
        uname = string.replace(uname, unicode("é", "utf-8"), "e")
        uname = string.replace(uname, unicode("Ġ", "utf-8"), "G")
        uname = string.replace(uname, unicode("h̨", "utf-8"), "h")
        uname = string.replace(uname, unicode("H̨", "utf-8"), "H")
        uname = string.replace(uname, unicode("H̱", "utf-8"), "H")
        uname = string.replace(uname, unicode("ı", "utf-8"), "i")
        uname = string.replace(uname, unicode("í", "utf-8"), "i")
        uname = string.replace(uname, unicode("ī", "utf-8"), "i")
        uname = string.replace(uname, unicode("İ", "utf-8"), "I")
        uname = string.replace(uname, unicode("ł", "utf-8"), "l")
        uname = string.replace(uname, unicode("Ł", "utf-8"), "L")
        uname = string.replace(uname, unicode("ń", "utf-8"), "n")
        uname = string.replace(uname, unicode("ñ", "utf-8"), "n")
        uname = string.replace(uname, unicode("ò", "utf-8"), "o")
        uname = string.replace(uname, unicode("ó", "utf-8"), "o")
        uname = string.replace(uname, unicode("ộ", "utf-8"), "o")
        uname = string.replace(uname, unicode("ŏ", "utf-8"), "eo") # Seoul
        uname = string.replace(uname, unicode("ō", "utf-8"), "o")
        uname = string.replace(uname, unicode("Ō", "utf-8"), "O")
        uname = string.replace(uname, unicode("ş", "utf-8"), "s")
        uname = string.replace(uname, unicode("Ş", "utf-8"), "S")
        uname = string.replace(uname, unicode("ţ", "utf-8"), "t")
        uname = string.replace(uname, unicode("Ţ", "utf-8"), "T")
        uname = string.replace(uname, unicode("ū", "utf-8"), "u")
        uname = string.replace(uname, unicode("ŭ", "utf-8"), "u")
        uname = string.replace(uname, unicode("ú", "utf-8"), "u")
        uname = string.replace(uname, unicode("ü", "utf-8"), "u")
        uname = string.replace(uname, unicode("Ŭ", "utf-8"), "U")
        uname = string.replace(uname, unicode("ź", "utf-8"), "z")

        # use more common names
        uname = string.replace(uname, "al-Basrah", "Basra")
        uname = string.replace(uname, "al-Madinah", "Medina")
        uname = string.replace(uname, "al-Mawsil", "Mosul")
        uname = string.replace(uname, "Gazzah", "Gaza")
        uname = string.replace(uname, "Gizeh", "Giza")
        uname = string.replace(uname, "Jiddah", "Jeddah")
        uname = string.replace(uname, "Nuremberg", "Nurnberg")
        uname = string.replace(uname, "Peking", "Beijing")
        uname = string.replace(uname, "Taibei", "Taipei")

        try:
            return uname.encode("ascii", "strict")
        except UnicodeEncodeError:
            sys.stderr.write("Failed conversion:  %s\n" % name)


def read(file):
    chars = open(file, 'r').read()

    # kill apparent inadvertent linebreaks
    chars = string.replace(chars, "\015\n", "")

    lines = chars.split("\n")
    return lines

def parseint(s):
    """Parse an int, default to 0 for easy comparison"""
    try: return int(s)
    except: return 0

def parsecoord(s):
    """Parse a latitude/longitude, default to None, won't be used in comparison"""
    try: return int(s)
    except: return None

def compile(lines):
    locs = []
    for line in lines:
        segs = line.split("\t")

        try:
            id = segs[0]
            name = segs[1]
            aliases = string.split(segs[2], ",")
            cat = segs[4]
            pop = parseint(segs[5])
            lat = parsecoord(segs[6])
            lon = parsecoord(segs[7])
            country = segs[8]
            region = segs[9]

            loc = Location(id, name, aliases, cat, pop, lat, lon, country, region)
            locs.append(loc) 
        except IndexError:
            pass # should be EOF

    return locs

def filtercat(locs, cat):
    return filter(lambda x: x.cat == cat, locs)

def sortpop(locs):
    return sorted(locs, cmp=lambda x,y: cmp(x.pop, y.pop), reverse=True)


if __name__ == "__main__":
    try:
        datafile = sys.argv[1]
    except IndexError:
        print "Usage:  %s <datafile> <outputfile>" % sys.argv[0]
    lines = read(datafile)
    locs = compile(lines)
    locs = filtercat(locs, "locality")
    for loc in sortpop(locs):
        print loc
