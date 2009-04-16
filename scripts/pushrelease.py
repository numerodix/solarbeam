#!/usr/bin/env python
#
# Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Make a release from a tag and push to sf.net

import os
import subprocess
import sys
import shutil
import tempfile

UNIXTITLE = "solarbeam"
SF_USER = "numerodix"


def invoke(cwd, args):
    print(">>>>> Running %s :: %s" % (cwd, " ".join(args)))
    popen = subprocess.Popen(args, cwd=cwd,
                 stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    (out, _err) = popen.communicate()
    out = str(out).strip()
    return popen.returncode, out

def git_clone(path, newpath):
    args = ["git", "clone", path]
    (code, out) = invoke(newpath, args)
    print(out)
    if code > 0: sys.exit(1)

def git_checkout(path, commit):
    args = ["git", "checkout", commit]
    (code, out) = invoke(path, args)
    print(out)
    if code > 0: sys.exit(1)

def make_zip(path):
    args = ["make", "zip"]
    (code, out) = invoke(path, args)
    print(out)
    if code > 0: sys.exit(1)

def push_to_sf(file):
    args = ["rsync", "-avP", "-e", "ssh",
            "%s" % file, 
            "%s@frs.sourceforge.net:uploads/" % SF_USER]
    (code, out) = invoke(os.getcwd(), args)
    print(out)
    if code > 0: sys.exit(1)

def make(commit):
    td = tempfile.mkdtemp(prefix=".%spkg" % UNIXTITLE)

    git_clone(os.getcwd(), td)
    gitpath = os.path.join(td, UNIXTITLE)
    git_checkout(gitpath, commit)

    make_zip(gitpath)
    try:
        os.makedirs(os.path.join(os.getcwd(), "dist"))
    except:
        pass
    target = os.path.join("dist", "%s-%s.zip" % (UNIXTITLE, commit))
    os.rename(os.path.join(gitpath, 
                           os.path.join("dist", "%s.zip" % UNIXTITLE)),
              os.path.join(os.getcwd(), 
                           target))

    shutil.rmtree(td)

    return target

def printhelp(full=False):
    ss = []
    if full:
        ss.append("Usage:  %s <tag name>\n" % sys.argv[0])
        ss.append("Tag commit:")
        ss.append(" * run scripts/tag.py\n")
        ss.append("Working order:")
        ss.append(" * clone git repo in /tmp")
        ss.append(" * checkout selected tag name")
        ss.append(" * make zip")
        ss.append(" * push to sf.net")
    ss.append("\nManual steps:")
    ss.append(" * update web/releases")
    ss.append(" * run scripts/genreleasepage.py")
    ss.append(" * make webup")
    s = '\n'.join(ss)
    print(s)


if __name__ == "__main__":
    try:
        version = sys.argv[1]
    except IndexError:
        printhelp(full=True)
        sys.exit()

    file = make(version)
    push_to_sf(file)
    printhelp()
