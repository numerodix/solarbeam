#!/usr/bin/env python
#
# Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Generates html for release page from template.

import os
import re
import string
import subprocess
import sys
import time

APPTITLE = "SolarBeam"
UNIXTITLE = "solarbeam"
RELEASES_TEMPLATE = "web/releases"
RELEASES_HTML = "web/releases.php"


class Release(object):
    def __init__(self, version, date, features, bugfixes):
        self.version = version
        self.date = date
        self.features = features
        self.bugfixes = bugfixes


def invoke(cwd, args):
    popen = subprocess.Popen(args, cwd=cwd,
                 stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    (out, _err) = popen.communicate()
    out = str(out).strip()
    return popen.returncode, out

def git_getcommitdate(commit):
    args = ["git", "log", commit, "--pretty=format:%at"]
    (code, out) = invoke(".", args)
    val = out.split('\n')[0]
    t = time.localtime(float(val))
    return time.strftime("%d.%m.%Y", t)

def parse_releases():
    s = open(RELEASES_TEMPLATE).read()
    ss = s.split('\n\n')

    releases = []
    for s in ss:
        r = re.search("^([0-9.]+)", s)
        version = r.group(0)

        features = []
        rs = re.findall("([+].*)", s)
        for r in rs:
            features.append(r[1:].strip())

        bugfixes = []
        rs = re.findall("([*].*)", s)
        for r in rs:
            bugfixes.append(r[1:].strip())

        date = git_getcommitdate(version)
        releases.append(Release(version, date, features, bugfixes))

    return releases

def write_releases(releases):
    t = time.localtime()
    t_s = "%s.%s.%s" % (t.tm_mday, t.tm_mon, t.tm_year)

    ss = ['<? // generated with %s on %s ?>\n' % (sys.argv[0], t_s)]
    for (i, rel) in enumerate(releases):
        ss.append('<div class="releasetitle">')
        ss.append('\t<a href="javascript:Collapse(\'release-%s\')">' % rel.version)
        ss.append('\t\t<img src="imgs/collapse.png" alt="collapse"/>')
        ss.append('\t\t%s %s' % (APPTITLE, rel.version))
        ss.append('\t</a> - %s' % rel.date)
        ss.append('</div>')

        collapse = ""
        if i > 0:
            collapse = ' style="display: none;"'

        ss.append('<div id="release-%s"%s>' % (rel.version, collapse))
        ss.append('\t<div class="releasecontent">')
        if rel.features:
            ss.append('\t\t<p>New features:</p>')
            ss.append('\t\t<ul>')
            for f in rel.features:
                ss.append('\t\t\t<li>%s</li>' % f)
            ss.append('\t\t</ul>')
        if rel.bugfixes:
            ss.append('\t\t<p>Bugs fixed:</p>')
            ss.append('\t\t<ul>')
            for f in rel.bugfixes:
                ss.append('\t\t\t<li>%s</li>' % f)
            ss.append('\t\t</ul>')
        ss.append('\t\t<p class="download">')
        ss.append('\t\t\t<a href="http://downloads.sourceforge.net/%s/%s-%s.zip">'
                   % (UNIXTITLE, UNIXTITLE, rel.version))
        ss.append('\t\t\t\t<img src="imgs/download.png" alt="download"/>')
        ss.append('\t\t\t\tDownload')
        ss.append('\t\t\t</a>')
        ss.append('\t\t</p>')
        ss.append('\t</div>')
        ss.append('</div>')

    ss = ['\t\t%s' % s for s in ss]
    s = '\n'.join(ss)

    open(RELEASES_HTML, 'w').write(s)
    

if __name__ == "__main__":
    releases = parse_releases()
    write_releases(releases)
