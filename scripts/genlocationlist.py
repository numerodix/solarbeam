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
        self.aliases = aliases
        self.pop = pop
        self.cat = cat
        self.lat = lat
        self.lon = lon
        self.country = country
        self.region = region

        self.name = None
        self.setname(name) # set name last

        self.timezone = None # init timezone field
    def __str__(self):
        s = "%s\t\t%s\t\t%s\t\t    %s" % (self.name, self.country,
                                          self.region,
                                          self.timezone)
        return s
    def getcountry(self):
        c = self.country

        c = string.replace(c, "El Salvador", "ElS")
        c = string.replace(c, "Korea (North)", "NKo")
        c = string.replace(c, "Korea (South)", "SKo")
        c = string.replace(c, "South Africa", "SA")
        c = string.replace(c, "United Arab Emirates", "UAE")
        c = string.replace(c, "United Kingdom", "UK")
        c = string.replace(c, "United States of America", "USA")
        
        return string.strip(c)
    def setname(self, name):
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
        uname = string.replace(uname, unicode("æ", "utf-8"), "a")
        uname = string.replace(uname, unicode("Æ", "utf-8"), "A")
        uname = string.replace(uname, unicode("å", "utf-8"), "a")
        uname = string.replace(uname, unicode("Å", "utf-8"), "A")
        uname = string.replace(uname, unicode("ầ", "utf-8"), "a")
        uname = string.replace(uname, unicode("ạ", "utf-8"), "a")
        uname = string.replace(uname, unicode("ẵ", "utf-8"), "a")
        uname = string.replace(uname, unicode("ə", "utf-8"), "a")
        uname = string.replace(uname, unicode("Å", "utf-8"), "A")
        uname = string.replace(uname, unicode("Ç", "utf-8"), "C")
        uname = string.replace(uname, unicode("Č", "utf-8"), "C")
        uname = string.replace(uname, unicode("ç", "utf-8"), "c")
        uname = string.replace(uname, unicode("đ", "utf-8"), "d")
        uname = string.replace(uname, unicode("d̨", "utf-8"), "d")
        uname = string.replace(uname, unicode("D̨", "utf-8"), "D")
        uname = string.replace(uname, unicode("Đ", "utf-8"), "D")
        uname = string.replace(uname, unicode("ế", "utf-8"), "e")
        uname = string.replace(uname, unicode("ē", "utf-8"), "e")
        uname = string.replace(uname, unicode("ě", "utf-8"), "e")
        uname = string.replace(uname, unicode("ê", "utf-8"), "e")
        uname = string.replace(uname, unicode("è", "utf-8"), "e")
        uname = string.replace(uname, unicode("ę", "utf-8"), "e")
        uname = string.replace(uname, unicode("é", "utf-8"), "e")
        uname = string.replace(uname, unicode("ğ", "utf-8"), "g")
        uname = string.replace(uname, unicode("Ġ", "utf-8"), "G")
        uname = string.replace(uname, unicode("h̨", "utf-8"), "h")
        uname = string.replace(uname, unicode("H̨", "utf-8"), "H")
        uname = string.replace(uname, unicode("H̱", "utf-8"), "H")
        uname = string.replace(uname, unicode("ı", "utf-8"), "i")
        uname = string.replace(uname, unicode("ì", "utf-8"), "i")
        uname = string.replace(uname, unicode("í", "utf-8"), "i")
        uname = string.replace(uname, unicode("ī", "utf-8"), "i")
        uname = string.replace(uname, unicode("İ", "utf-8"), "I")
        uname = string.replace(uname, unicode("ķ", "utf-8"), "k")
        uname = string.replace(uname, unicode("Ķ", "utf-8"), "K")
        uname = string.replace(uname, unicode("ł", "utf-8"), "l")
        uname = string.replace(uname, unicode("Ł", "utf-8"), "L")
        uname = string.replace(uname, unicode("ń", "utf-8"), "n")
        uname = string.replace(uname, unicode("ñ", "utf-8"), "n")
        uname = string.replace(uname, unicode("ņ", "utf-8"), "n")
        uname = string.replace(uname, unicode("ø", "utf-8"), "o")
        uname = string.replace(uname, unicode("Ø", "utf-8"), "O")
        uname = string.replace(uname, unicode("ö", "utf-8"), "o")
        uname = string.replace(uname, unicode("Ö", "utf-8"), "O")
        uname = string.replace(uname, unicode("ò", "utf-8"), "o")
        uname = string.replace(uname, unicode("ó", "utf-8"), "o")
        uname = string.replace(uname, unicode("ố", "utf-8"), "o")
        uname = string.replace(uname, unicode("ồ", "utf-8"), "o")
        uname = string.replace(uname, unicode("ộ", "utf-8"), "o")
        uname = string.replace(uname, unicode("ŏ", "utf-8"), "eo") # Seoul
        uname = string.replace(uname, unicode("ơ", "utf-8"), "o")
        uname = string.replace(uname, unicode("ō", "utf-8"), "o")
        uname = string.replace(uname, unicode("Ō", "utf-8"), "O")
        uname = string.replace(uname, unicode("š", "utf-8"), "s")
        uname = string.replace(uname, unicode("ŗ", "utf-8"), "r")
        uname = string.replace(uname, unicode("ş", "utf-8"), "s")
        uname = string.replace(uname, unicode("Š", "utf-8"), "S")
        uname = string.replace(uname, unicode("Ş", "utf-8"), "S")
        uname = string.replace(uname, unicode("ţ", "utf-8"), "t")
        uname = string.replace(uname, unicode("Ṯ", "utf-8"), "T")
        uname = string.replace(uname, unicode("Ţ", "utf-8"), "T")
        uname = string.replace(uname, unicode("ū", "utf-8"), "u")
        uname = string.replace(uname, unicode("ŭ", "utf-8"), "u")
        uname = string.replace(uname, unicode("ú", "utf-8"), "u")
        uname = string.replace(uname, unicode("ũ", "utf-8"), "u")
        uname = string.replace(uname, unicode("ü", "utf-8"), "u")
        uname = string.replace(uname, unicode("Ŭ", "utf-8"), "U")
        uname = string.replace(uname, unicode("ź", "utf-8"), "z")
        uname = string.replace(uname, unicode("ž", "utf-8"), "z")
        uname = string.replace(uname, unicode("Ẕ", "utf-8"), "Z")

        # use more common names
        uname = string.replace(uname, "'Adan", "Adan")
        uname = string.replace(uname, "'Ajman", "Ajman")
        uname = string.replace(uname, "'Amman", "Amman")
        uname = string.replace(uname, "al-'Ayun", "El Aaiun")
        uname = string.replace(uname, "al-'Ayn", "Al Ain")
        uname = string.replace(uname, "al-'Amarah", "Amarah")
        uname = string.replace(uname, "al-Basrah", "Basra")
        uname = string.replace(uname, "ad-Dammam", "Damman")
        uname = string.replace(uname, "al-Madinah", "Medina")
        uname = string.replace(uname, "al-Mawsil", "Mosul")
        uname = string.replace(uname, "an-Najaf", "Najaf")
        uname = string.replace(uname, "at-Ta'if", "Taif")
        uname = string.replace(uname, "Bayrut", "Beirut")
        uname = string.replace(uname, "Dnipropetrovs'k", "Dnipropetrovsk")
        uname = string.replace(uname, "Fes", "Fez")
        uname = string.replace(uname, "Fredrikstad-Sarpsborg", "Fredrikstad")
        uname = string.replace(uname, "Gazzah", "Gaza")
        uname = string.replace(uname, "Gizeh", "Giza")
        uname = string.replace(uname, "Homjel'", "Homjel")
        uname = string.replace(uname, "Jiddah", "Jeddah")
        uname = string.replace(uname, "L'viv", "Lvov")
        uname = string.replace(uname, "Nuremberg", "Nurnberg")
        uname = string.replace(uname, "Peking", "Beijing")
        uname = string.replace(uname, "Porsgrunn-Skien", "Porsgrunn")
        uname = string.replace(uname, "Pyeongyang", "Pyongyang")
        uname = string.replace(uname, "s-Gravenhage", "Den Haag")
        uname = string.replace(uname, "Stavanger-Sandnes", "Stavanger")
        uname = string.replace(uname, "Taibei", "Taipei")
        uname = string.replace(uname, "Ta'izz", "Taiz")
        uname = string.replace(uname, "Ulaanbaatar", "Ulan Bator")

        try:
            name = string.strip(uname.encode("ascii", "strict"))
            s = "%s (%s)" % (name, self.getcountry()[:3])
            self.name = s
        except UnicodeEncodeError:
            sys.stderr.write("%s  \t%s  \t%s   \tfailed conversion\n" % (self.pop,
                             name, self.country))


### READING ROUTINES


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
    try: 
        f = int(s) # see if it's an int
        return s
    except: return None

def compile(lines):
    locs = []
    for line in lines:
        segs = line.split("\t")

        try:
            id = segs[0]
            name = string.split(segs[1], ",")[0]  # formatting bug
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


### ALGEBRA ROUTINES


def killdupes(locs):
    d = {}
    for loc in locs:
        if loc.name not in d:
            d[loc.name] = loc
    return d.values()

def filtermissingpos(locs):
    return filter(lambda x: x.lat != None and x.lon != None, locs)

def filtercat(locs, cat):
    return filter(lambda x: x.cat == cat, locs)

def filtercountry(locs, country):
    return filter(lambda x: x.country == country, locs)

def filternotcountry(locs, country):
    return filter(lambda x: x.country != country, locs)

def sortpop(locs):
    return sorted(locs, cmp=lambda x,y: cmp(x.pop, y.pop), reverse=True)

def sortname(locs):
    return sorted(locs, cmp=lambda x,y: cmp(x.name, y.name))

def sortcountry(locs):
    return sorted(locs, cmp=lambda x,y: cmp(x.country, y.country))


### CODEGEN ROUTINES


zones = {
    "Afghanistan": "Asia/Kabul",
    "Albania": "Europe/Tirane",
    "Algeria": "Asia/Algiers",
    "Angola": "Africa/Luanda",
    "Armenia": "Asia/Yerevan",
    "Austria": "Europe/Vienna",
    "Azerbaijan": "Asia/Baku",
    "Bahamas": "America/Nassau",
    "Bangladesh": "Asia/Dhaka",
    "Belarus": "Europe/Minsk",
    "Belgium": "Europe/Brussels",
    "Benin": "Africa/Porto-Novo",
    "Bolivia": "America/La_Paz",
    "Bosnia and Herzegovina": "Europe/Sarajevo",
    "Botswana": "Africa/Gaborone",
    "Bulgaria": "Europe/Sofia",
    "Burkina Faso": "Africa/Ouagadougou",
    "Burundi": "Africa/Bujumbura",
    "Cambodia": "Asia/Phnom_Penh",
    "Cameroon": "Africa/Douala",
    "Central African Republic": "Africa/Bangui",
    "Chad": "Africa/Ndjamena",
    "Chile": "America/Santiago",
    "China": "Asia/Shanghai",
    "Colombia": "America/Bogota",
    "Congo": "Africa/Brazzaville",
    "Croatia": "Europe/Zagreb",
    "Cuba": "America/Havana",
    "Cyprus": "Europe/Nicosia",
    "Czech Republic": "Europe/Prague",
    "Djibouti": "Africa/Djibouti",
    "Denmark": "Europe/Copenhagen",
    "Dominican Republic": "America/Santo_Domingo",
    "Ecuador": "America/Guayaquil",
    "Equatorial Guinea": "Africa/Malabo",
    "Egypt": "Africa/Cairo",
    "El Salvador": "America/El_Salvador",
    "Eritrea": "Africa/Asmara",
    "Estonia": "Europe/Tallinn",
    "Ethiopia": "Africa/Addis_Ababa",
    "Finland": "Europe/Helsinki",
    "France": "Europe/Paris",
    "Gabon": "Africa/Libreville",
    "Gambia": "Africa/Banjul",
    "Georgia": "Asia/Tbilisi",
    "Germany": "Europe/Berlin",
    "Ghana": "Africa/Accra",
    "Greece": "Europe/Athens",
    "Guinea": "Africa/Conakry",
    "Guinea-Bissau": "Africa/Bissau",
    "Guatemala": "America/Guatemala",
    "Guyana": "America/Guyana",
    "Haiti": "America/Port-au-Prince",
    "Honduras": "America/Tegucigalpa",
    "Hungary": "Europe/Budapest",
    "Iran": "Asia/Tehran",
    "Iraq": "Asia/Baghdad",
    "Ireland": "Europe/Dublin",
    "Israel": "Asia/Tel_Aviv",
    "Italy": "Europe/Rome",
    "Ivory Coast": "Africa/Abidjan",
    "Jamaica": "America/Jamaica",
    "Japan": "Asia/Tokyo",
    "Jordan": "Asia/Amman",
    "Kenya": "Africa/Nairobi",
    "Korea (North)": "Asia/Pyongyang",
    "Korea (South)": "Asia/Seoul",
    "Kosovo": "Europe/Belgrade",
    "Kyrgyzstan": "Asia/Bishkek",
    "Laos": "Asia/Vientiane",
    "Latvia": "Europe/Riga",
    "Lebanon": "Asia/Beirut",
    "Lesotho": "America/Maseru",
    "Liberia": "Africa/Monrovia",
    "Libya": "Africa/Tripoli",
    "Lithuania": "Europe/Vilnius",
    "Macedonia": "Europe/Skopje", 
    "Madagascar": "Indian/Antananarivo", 
    "Malawi": "Africa/Blantyre",
    "Malaysia": "Asia/Kuala_Lumpur",
    "Mali": "Africa/Bamako",
    "Mauritania": "Africa/Nouakchott",
    "Moldova": "Europe/Chisinau",
    "Mongolia": "Asia/Ulan_Bator",
    "Morocco": "Africa/Casablanca",
    "Mozambique": "Africa/Maputo",
    "Myanmar": "Asia/Rangoon",
    "Namibia": "Africa/Windhoek",
    "Nepal": "Asia/Kathmandu",
    "Netherlands": "Europe/Amsterdam",
    "New Zealand": "Pacific/Auckland",
    "Nicaragua": "America/Managua",
    "Niger": "Africa/Niamey",
    "Nigeria": "Africa/Lagos",
    "Norway": "Europe/Oslo",
    "Oman": "Asia/Muscat",
    "Pakistan": "Asia/Karachi",
    "Palestine": "Asia/Gaza",
    "Panama": "America/Panama",
    "Paraguay": "America/Asuncion",
    "Peru": "America/Lima",
    "Philippines": "Asia/Manila",
    "Poland": "Europe/Warsaw",
    "Portugal": "Europe/Lisbon",
    "Puerto Rico": "America/Puerto_Rico",
    "Qatar": "Asia/Qatar",
    "Romania": "Europe/Bucharest",
    "Rwanda": "Africa/Kigali",
    "Saudi Arabia": "Asia/Riyadh",
    "Senegal": "Africa/Dakar",
    "Serbia": "Europe/Belgrade",
    "Sierra Leone": "Africa/Freetown",
    "Singapore": "Asia/Singapore",
    "Slovakia": "Europe/Bratislava",
    "Slovenia": "Europe/Ljubljana",
    "South Africa": "Africa/Johannesburg",
    "Somalia": "Africa/Mogadishu",
    "Spain": "Europe/Madrid",
    "Sri Lanka": "Asia/Colombo",
    "Sudan": "Africa/Khartoum",
    "Suriname": "America/Paramaribo",
    "Sweden": "Europe/Stockholm",
    "Switzerland": "Europe/Zurich",
    "Syria": "Asia/Damascus",
    "Taiwan": "Asia/Taipei",
    "Tajikistan": "Asia/Dushanbe",
    "Tanzania": "Africa/Dar_es_Salaam",
    "Thailand": "Asia/Bangkok",
    "Togo": "Africa/Lome",
    "Tunisia": "Africa/Tunis",
    "Turkey": "Asia/Istanbul",
    "Turkmenistan": "Asia/Ashgabat",
    "Uganda": "Africa/Kampala",
    "Ukraine": "Europe/Kiev",
    "United Arab Emirates": "Asia/Dubai",
    "United Kingdom": "Europe/London",
    "Uruguay": "America/Montevideo",
    "Uzbekistan": "Asia/Tashkent",
    "Venezuela": "America/Caracas",
    "Vietnam": "Asia/Ho_Chi_Minh",
    "Western Sahara": "Africa/El_Aaiun",
    "Yemen": "Asia/Aden",
    "Zambia": "Africa/Lusaka",
    "Zimbabwe": "Africa/Harare",
}
def gettimezone(loc):
    try:
        try:
            return zones[loc.country]
        except KeyError:
            return zones[loc.name]
    except KeyError:
        return "UTC"
        sys.stderr.write("%s  \t%s  \t%s   \tno timezone\n" % (loc.pop,
                           loc.name, loc.country))

def getdeg(val):
    v = int(val) / 100. # shift decimal place by 2
    val = abs(v)
    sign = True
    if v < 0:
        sign = False
    val *= 3600.
    deg = int(val / 3600.)
    min = int((val - (deg*3600)) / 60.)
    sec = int(val - (deg*3600) - (min*60))
    return (sign, deg, min, sec)

def getlat(loc):
    (sign, deg, min, sec) = getdeg(loc.lat)
    dir = "PositionDirection.North"
    if not sign:
        dir = "PositionDirection.South"
    return (dir, deg, min, sec)

def getlon(loc):
    (sign, deg, min, sec) = getdeg(loc.lon)
    dir = "PositionDirection.East"
    if not sign:
        dir = "PositionDirection.West"
    return (dir, deg, min, sec)

def codegen(loc):
    (ladir, ladeg, lamin, lasec) = getlat(loc)
    (lodir, lodeg, lomin, losec) = getlon(loc)
    loc.timezone = gettimezone(loc)
    s = '\t\t\tlist.Add("%s", "%s",\n\t\t\t\tnew Position(%s, %s, %s, %s,\n'
    s += '\t\t\t\t\t%s, %s, %s, %s));'
    f = s % (loc.name, loc.timezone, 
             ladir, ladeg, lamin, lasec,
             lodir, lodeg, lomin, losec)
#    s = 'try {\n%s\n} catch (ArgumentException) {\nConsole.WriteLine("%s");\n}'
#    f = s % (f, loc.name)
    return f

if __name__ == "__main__":
    try:
        datafile = sys.argv[1]
    except IndexError:
        print "Usage:  %s <datafile> <outputfile>" % sys.argv[0]
        sys.exit()
    
    # read locations
    lines = read(datafile)
    locs = compile(lines)

    # do some filtering
    locs = filtercat(locs, "locality")
    locs = killdupes(locs)
    locs = sortpop(locs)
    locs = filtermissingpos(locs)

    # 2000 world, 25 Norway
    final_locs = filternotcountry(locs, "Norway")[:2000]
    nor_locs = filtercountry(locs, "Norway")[:25]
    final_locs.extend(nor_locs)

    #final_locs = sortname(final_locs)
    final_locs = sortcountry(final_locs)
    for loc in final_locs:
        codegen(loc)
        print loc
