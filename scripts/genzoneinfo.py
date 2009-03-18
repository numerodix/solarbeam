#!/usr/bin/env python
#
# Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Fetches the latest zoneinfo dbase and generates a zoneinfo file.

import os
import shutil
import time


workdir = "tz"
filter_out = [
    "factory",
    "solar87",
    "solar88",
    "solar89",
]
target_dir = ".."
target_file = "zoneinfo"


def fetch():
    os.system("wget 'ftp://elsie.nci.nih.gov/pub/tzdata*.tar.gz'")
    os.system("gzip -dc tzdata*.tar.gz | tar -xf -")

def gen():
    lines = []

    entries = os.listdir(".")
    entries.sort()
    for entry in entries:
        if os.path.isfile(entry):
            (root, ext) = os.path.splitext(entry)
            if ext == "" and root not in filter_out:
                
                lines.extend(open(root, 'r').readlines())

    lines = filter(lambda x: x[0] != "#", lines)
    lines = filter(lambda x: x != "\t", lines)
    lines = filter(lambda x: x != "\n", lines)
    lines = filter(lambda x: x != " \n", lines)

    # prepend gen info
    t = time.localtime()
    t_s = "on %s.%s.%s\n\n" % (t.tm_mday, t.tm_mon, t.tm_year)
    lines.insert(0, "# Generated with genzoneinfo.py from the SolarBeam distribution %s" % t_s)

    txt = "".join(lines)

    return txt

def write(txt):
    try:
        os.mkdir(target_dir)
    except:
        pass

    open(os.path.join(target_dir, target_file), 'w').write(txt)


if __name__ == "__main__":
    cwd = os.getcwd()

    os.mkdir(workdir)
    os.chdir(workdir)

    fetch()
    txt = gen()
    
    os.chdir(cwd)

    write(txt)

    shutil.rmtree(workdir)
