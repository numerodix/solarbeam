#!/usr/bin/env python
# -*- coding: UTF-8 -*-
#
# Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Generates c# code for LocationList using location data file from
# http://www.world-gazetteer.com.

import string
import sys
import time


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
        fst = str(self.name)
        snd = str(self.country)
        thd = str(self.region)
        frh = str(self.pop)
        fih = str(self.timezone)
        s = "%s %s %s %s %s" % (fst.ljust(25), snd.ljust(20), thd.ljust(20),
                            frh.ljust(20), fih)
        return s
    def getcountry(self):
        c = self.country

        c = string.replace(c, "El Salvador", "ElS")
        c = string.replace(c, "Korea (North)", "NKo")
        c = string.replace(c, "Korea (South)", "SKo")
        c = string.replace(c, "New Zealand", "NZ")
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

        # use more common names (english wikipedia as reference)
        uname = string.replace(uname, "'Adan", "Aden")
        uname = string.replace(uname, "'Ajman", "Ajman")
        uname = string.replace(uname, "'Amman", "Amman")
        uname = string.replace(uname, "ad-Dammam", "Damman")
        uname = string.replace(uname, "ad-Diwaniyah", "Ad Diwaniyah")
        uname = string.replace(uname, "ad-Du'ayn", "Ad Du'ayn")
        uname = string.replace(uname, "al-'Ayun", "El Aaiun")
        uname = string.replace(uname, "al-'Ayn", "Al Ain")
        uname = string.replace(uname, "al-'Amarah", "Amarah")
        uname = string.replace(uname, "al-Basrah", "Basra")
        uname = string.replace(uname, "al-Fallujah", "Fallujah")
        uname = string.replace(uname, "al-Fasir", "Al Fasir")
        uname = string.replace(uname, "al-Harj", "Al-Kharj")
        uname = string.replace(uname, "al-Hartum Bahri", "Al Khurtum Bahri")
        uname = string.replace(uname, "al-Hillah", "Al Hillah")
        uname = string.replace(uname, "al-Hufuf", "Hofuf")
        uname = string.replace(uname, "al-Hums", "Al Khums")
        uname = string.replace(uname, "al-Jilfah", "Al-Jilfah")
        uname = string.replace(uname, "al-Kut", "Kut")
        uname = string.replace(uname, "al-Ladiqiyah", "Al-Ladiqiyah")
        uname = string.replace(uname, "al-Madinah", "Medina")
        uname = string.replace(uname, "al-Mawsil", "Mosul")
        uname = string.replace(uname, "al-Mubarraz", "Al-Mubarraz")
        uname = string.replace(uname, "al-Mukalla", "Al Mukalla")
        uname = string.replace(uname, "al-Qadarif", "Al Qadarif")
        uname = string.replace(uname, "al-Ubayyid", "Al-Ubayyid")
        uname = string.replace(uname, "an-Najaf", "Najaf")
        uname = string.replace(uname, "an-Nasiriyah", "Nasiriyah")
        uname = string.replace(uname, "ar-Ramadi", "Ramadi")
        uname = string.replace(uname, "ar-Raqqah", "Ar-Raqqah")
        uname = string.replace(uname, "ar-Rayyan", "Ar Rayyan")
        uname = string.replace(uname, "ar-Russayfah", "Ar-Russayfah")
        uname = string.replace(uname, "as-Sib", "As Sib")
        uname = string.replace(uname, "as-Sulaymaniyah", "Sulaymaniyah")
        uname = string.replace(uname, "at-Ta'if", "Taif")
        uname = string.replace(uname, "az-Zarqa'", "Zarqa")
        uname = string.replace(uname, "az-Zawiyah", "Az Zawiyah")
        uname = string.replace(uname, "Aomen", "Macau")
        uname = string.replace(uname, "Ba'qubah", "Baqubah")
        uname = string.replace(uname, "Bandar-e 'Abbas", "Bandar Abbas")
        uname = string.replace(uname, "Bayrut", "Beirut")
        uname = string.replace(uname, "Dniprodzerzhyns'k", "Dniprodzerzhynsk")
        uname = string.replace(uname, "Dnipropetrovs'k", "Dnipropetrovsk")
        uname = string.replace(uname, "Elx", "Elche")
        uname = string.replace(uname, "Fes", "Fez")
        uname = string.replace(uname, "Fredrikstad-Sarpsborg", "Fredrikstad")
        uname = string.replace(uname, "Gazzah", "Gaza")
        uname = string.replace(uname, "Gizeh", "Giza")
        uname = string.replace(uname, "Ha Noi", "Hanoi")
        uname = string.replace(uname, "Homjel'", "Homjel")
        uname = string.replace(uname, "Ivano-Frankivs'k", "Ivano-Frankivsk")
        uname = string.replace(uname, "Jiddah", "Jeddah")
        uname = string.replace(uname, "Karbala'", "Karbala")
        uname = string.replace(uname, "Khmel'nyts'kyy", "Khmelnytskyi")
        uname = string.replace(uname, "Kobenhavn", "Copenhagen")
        uname = string.replace(uname, "Luhans'k", "Luhansk")
        uname = string.replace(uname, "L'viv", "Lvov")
        uname = string.replace(uname, "Mariupol'", "Mariupol")
        uname = string.replace(uname, "Nuremberg", "Nurnberg")
        uname = string.replace(uname, "Qaragandy", "Karaganda")
        uname = string.replace(uname, "Qostanay", "Kostanay")
        uname = string.replace(uname, "Peking", "Beijing")
        uname = string.replace(uname, "Phnum Penh", "Phnom Penh")
        uname = string.replace(uname, "Porsgrunn-Skien", "Porsgrunn")
        uname = string.replace(uname, "Pyeongyang", "Pyongyang")
        uname = string.replace(uname, "s-Gravenhage", "Den Haag")
        uname = string.replace(uname, "Sevastopol'", "Sevastopol")
        uname = string.replace(uname, "Simferopol'", "Simferopol")
        uname = string.replace(uname, "Stavanger-Sandnes", "Stavanger")
        uname = string.replace(uname, "Taibei", "Taipei")
        uname = string.replace(uname, "Ta'izz", "Taiz")
        uname = string.replace(uname, "Taegu", "Daegu")
        uname = string.replace(uname, "Tel Aviv-Yafo", "Tel Aviv")
        uname = string.replace(uname, "Ternopil'", "Ternopil")
        uname = string.replace(uname, "Ulaanbaatar", "Ulan Bator")

        # do this last to prevent overwriting all common prefixes
        uname = string.replace(uname, "al-", "Al 'Aziziyah")

        try:
            name = string.strip(uname.encode("ascii", "strict"))
            country = self.getcountry()

            s = name
            if country != "":
                s = "%s (%s)" % (name, country[:3])

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
    ls = []
    for loc in locs:
        if loc.name not in d:
            d[loc.name] = loc
            ls.append(loc)
    return ls

def filtermissingpos(locs):
    return filter(lambda x: x.lat != None and x.lon != None, locs)

def filtercat(locs, cats):
    return filter(lambda x: x.cat in cats, locs)

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
    # States

    "Afghanistan": "Asia/Kabul",
    "Albania": "Europe/Tirane",
    "Algeria": "Africa/Algiers",
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
    "Aomen": "Asia/Macau",
    "Hongkong": "Asia/Hong_Kong",
    "Colombia": "America/Bogota",
    "Congo": "Africa/Brazzaville",
    "Costa Rica": "America/Costa_Rica",
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
    "India": "Asia/Kolkata",
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
    "Lesotho": "Africa/Maseru",
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
    "Papua New Guinea": "Pacific/Port_Moresby",
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

    # Regional
    
    "Western Australia": "Australia/Perth",
    # Australia +9:30
    "South Australia": "Australia/Adelaide",
    # Australia +10
    "Australian Capital Territory": "Australia/Canberra",
    "New South Wales": "Australia/Sydney",
    "Queensland": "Australia/Brisbane",
    "Victoria": "Australia/Melbourne",
    
    # Argentina
    "Buenos Aires": "America/Argentina/Buenos_Aires",
    "Buenos Aires (Arg)": "America/Argentina/Buenos_Aires",
    "Catamarca": "America/Argentina/Catamarca",
    "Chaco": "America/Argentina/Cordoba",
    "Córdoba": "America/Argentina/Cordoba",
    "Corrientes": "America/Argentina/Cordoba",
    "Entre Ríos": "America/Argentina/Cordoba",
    "Formosa": "America/Argentina/Cordoba",
    "Misiones": "America/Argentina/Cordoba",
    "Santa Fé": "America/Argentina/Cordoba",
    "Santiago del Estero": "America/Argentina/Cordoba",
    "Jujuy": "America/Argentina/Jujuy",
    "Mendoza": "America/Argentina/Mendoza",
    "Neuquén": "America/Argentina/Salta",
    "Salta": "America/Argentina/Salta",
    "San Juan": "America/Argentina/San_Juan",
    "San Luis": "America/Argentina/San_Luis",
    "Tucumán": "America/Argentina/Tucuman",

    # Brazil
    "Tocantins": "America/Araguaina",
    "Bahia": "America/Bahia",
    "Amapá": "America/Belem",
    "Ananindeua (Bra)": "America/Belem",
    "Pará": "America/Belem",
    "Roraima": "America/Boa_Vista",
    "Mato Grosso do Sul": "America/Campo_Grande",
    "Mato Grosso": "America/Cuiaba",
    "Ceará": "America/Fortaleza",
    "Maranhão": "America/Fortaleza",
    "Paraíba": "America/Fortaleza",
    "Piauí": "America/Fortaleza",
    "Rio Grande do Norte": "America/Fortaleza",
    "Alagoas": "America/Maceio",
    "Sergipe": "America/Maceio",
    "Manaus (Bra)": "America/Manaus",
    "Rondônia": "America/Porto_Velho",
    "Pernambuco": "America/Recife",
    "Acre": "America/Rio_Branco",
    "Santarem (Bra)": "America/Santarem",
    "Brasilia (Bra)": "America/Sao_Paulo",
    "Espírito Santo": "America/Sao_Paulo",
    "Goiás": "America/Sao_Paulo",
    "Minas Gerais": "America/Sao_Paulo",
    "Paraná": "America/Sao_Paulo",
    "Rio de Janeiro": "America/Sao_Paulo",
    "Rio Grande do Sul": "America/Sao_Paulo",
    "Santa Catarina": "America/Sao_Paulo",
    "São Paulo": "America/Sao_Paulo",

    # Canada
    "Alberta": "America/Edmonton",
    "Halifax (Can)": "America/Halifax",
    "Montreal (Can)": "America/Montreal",
    "Quebec (Can)": "America/Montreal",
    "Saskatoon (Can)": "America/Regina",
    "Hamilton (Can)": "America/Toronto",
    "Kitchener (Can)": "America/Toronto",
    "London (Can)": "America/Toronto",
    "Oshawa (Can)": "America/Toronto",
    "Ottawa (Can)": "America/Toronto",
    "Saint Catharines-Niagara (Can)": "America/Toronto",
    "Toronto (Can)": "America/Toronto",
    "Windsor (Can)": "America/Toronto",
    "Vancouver (Can)": "America/Vancouver",
    "Victoria (Can)": "America/Vancouver",
    "Manitoba": "America/Winnipeg",

    # Congo West +1
    "Bandundu": "Africa/Kinshasa",
    "Bas-Congo": "Africa/Kinshasa",
    "Équateur": "Africa/Kinshasa",
    "Kinshasa": "Africa/Kinshasa",
    # Congo East +2
    "Kasai-Occidental": "Africa/Lubumbashi",
    "Kasai-Oriental": "Africa/Lubumbashi",
    "Katanga": "Africa/Lubumbashi",
    "Maniema": "Africa/Lubumbashi",
    "Nord-Kivu": "Africa/Lubumbashi",
    "Haut-Congo": "Africa/Lubumbashi",
    "Sud-Kivu": "Africa/Lubumbashi",

    # Indonesia
    "Banten": "Asia/Jakarta",
    "Bengkulu": "Asia/Jakarta",
    "Jakarta": "Asia/Jakarta",
    "Jambi": "Asia/Jakarta",
    "Jawa Barat": "Asia/Jakarta",
    "Jawa Tengah": "Asia/Jakarta",
    "Jawa Timur": "Asia/Jakarta",
    "Lampung": "Asia/Jakarta",
    "Riau": "Asia/Jakarta",
    "Riau Kepulauan": "Asia/Jakarta",
    "Selatan": "Asia/Jakarta",
    "Sumatera Barat": "Asia/Jakarta",
    "Sumatera Selatan": "Asia/Jakarta",
    "Sumatera Utara": "Asia/Jakarta",
    "Yogyakarta": "Asia/Jakarta",
    "Maluku": "Asia/Jayapura",
    "Bali": "Asia/Makassar",
    "Nusa Tenggara Barat": "Asia/Makassar",
    "Nusa Tenggara Timur": "Asia/Makassar",
    "Sulawesi Selatan": "Asia/Makassar",
    "Sulawesi Tengah": "Asia/Makassar",
    "Sulawesi Tenggara": "Asia/Makassar",
    "Sulawesi Utara": "Asia/Makassar",
    "Kalimantan Barat": "Asia/Pontianak",
    "Kalimantan Selatan": "Asia/Makassar",
    "Kalimantan Tengah": "Asia/Pontianak",
    "Kalimantan Timur": "Asia/Pontianak",
    "Aceh": "Asia/Pontianak",

    # Kazakhstan Regional
    "Aqtobe (Kaz)": "Asia/Aqtobe",
    # Kazakhstan East +6
    "Almaty (Kaz)": "Asia/Almaty",
    "Astana (Kaz)": "Asia/Almaty",
    "Karaganda (Kaz)": "Asia/Almaty",
    "Kostanay (Kaz)": "Asia/Almaty",
    "Oskemen (Kaz)": "Asia/Almaty",
    "Pavlodar (Kaz)": "Asia/Almaty",
    "Petropavl (Kaz)": "Asia/Almaty",
    "Semey (Kaz)": "Asia/Almaty",
    "Shymkent (Kaz)": "Asia/Almaty",
    "Taraz (Kaz)": "Asia/Almaty",
    # Kazakhstan West +5
    "Oral (Kaz)": "Asia/Oral",

    # Mexico Central -6
    "Mexico": "America/Mexico_City", # catchall
    "Quintana Roo": "America/Cancun",
    "Campeche": "America/Merida",
    "Coahuila": "America/Monterrey",
    "Durango": "America/Monterrey",
    "Nuevo León": "America/Monterrey",
    "Tamaulipas": "America/Monterrey",
    # Mexico Mountain -7
    "Sinaloa": "America/Mazatlan",
    "Nayarit": "America/Mazatlan",
    "Chihuahua": "America/Chihuahua",
    "Sonora": "America/Hermosillo",
    # Mexico Pacific -8
    "Baja California": "America/Tijuana",

    # Russia
    "Burjatija": "Asia/Irkutsk",
    "Irkutsk": "Asia/Irkutsk",
    "Kamčatka": "Asia/Kamchatka",
    "Kemerovo": "Asia/Krasnoyarsk",
    "Krasnojarsk": "Asia/Krasnoyarsk",
    "Hanty-Mansija": "Asia/Novosibirsk",
    "Novosibirsk": "Asia/Novosibirsk",
    "Tomsk": "Asia/Novosibirsk",
    "Altaj": "Asia/Omsk",
    "Omsk": "Asia/Omsk",
    "Habarovsk": "Asia/Vladivostok",
    "Primorje": "Asia/Vladivostok",
    "Amur": "Asia/Yakutsk",
    "Čita": "Asia/Yakutsk",
    "Saha": "Asia/Yakutsk",
    "Baškortostan": "Asia/Yekaterinburg",
    "Čeljabinsk": "Asia/Yekaterinburg",
    "Kurgan": "Asia/Yekaterinburg",
    "Orenburg": "Asia/Yekaterinburg",
    "Perm": "Asia/Yekaterinburg",
    "Sverdlovsk": "Asia/Yekaterinburg",
    "Tjumen": "Asia/Yekaterinburg",
    "Kaliningrad": "Europe/Kaliningrad",
    "Arhangelsk": "Europe/Moscow",
    "Belgorod": "Europe/Moscow",
    "Brjansk": "Europe/Moscow",
    "Čečenija": "Europe/Moscow",
    "Čuvašija": "Europe/Moscow",
    "Dagestan": "Europe/Moscow",
    "Ivanovo": "Europe/Moscow",
    "Jaroslavl": "Europe/Moscow",
    "Kabardino-Balkarija": "Europe/Moscow",
    "Kaluga": "Europe/Moscow",
    "Karelija": "Europe/Moscow",
    "Komi": "Europe/Moscow",
    "Kostroma": "Europe/Moscow",
    "Krasnodar": "Europe/Moscow",
    "Kursk": "Europe/Moscow",
    "Lipeck": "Europe/Moscow",
    "Marij El": "Europe/Moscow",
    "Mordovija": "Europe/Moscow",
    "Moskau": "Europe/Moscow",
    "Moskovskaja Oblast": "Europe/Moscow",
    "Murmansk": "Europe/Moscow",
    "Nižnij Novgorod": "Europe/Moscow",
    "Novgorod": "Europe/Moscow",
    "Orjol": "Europe/Moscow",
    "Penza": "Europe/Moscow",
    "Pskov": "Europe/Moscow",
    "Rostov": "Europe/Moscow",
    "Rjazan": "Europe/Moscow",
    "Sankt Petersburg": "Europe/Moscow",
    "Smolensk": "Europe/Moscow",
    "Stavropol": "Europe/Moscow",
    "Tambov": "Europe/Moscow",
    "Tatarstan": "Europe/Moscow",
    "Tula": "Europe/Moscow",
    "Tver": "Europe/Moscow",
    "Uljanovsk": "Europe/Moscow",
    "Vladimir": "Europe/Moscow",
    "Vologda": "Europe/Moscow",
    "Voronež": "Europe/Moscow",
    "Samara": "Europe/Samara",
    "Udmurtija": "Europe/Samara",
    "Alanija": "Europe/Volgograd",
    "Astrahan": "Europe/Volgograd",
    "Kirov": "Europe/Volgograd",
    "Saratov": "Europe/Volgograd",
    "Volgograd": "Europe/Volgograd",

    # United States
    "Anchorage (USA)": "America/Anchorage",
    "Alabama": "America/Chicago",
    "Illinois": "America/Chicago",
    "Indiana": "America/Chicago",
    "Kansas": "America/Chicago",
    "Louisiana": "America/Chicago",
    "Memphis (USA)": "America/Chicago",
    "Minnesota": "America/Chicago",
    "Missouri": "America/Chicago",
    "Nashville (USA)": "America/Chicago",
    "Nebraska": "America/Chicago",
    "Oklahoma": "America/Chicago",
    "Texas": "America/Chicago",
    "Wisconsin": "America/Chicago",
    "Colorado": "America/Denver",
    "New Mexico": "America/Denver",
    "Michigan": "America/Detroit",
    "Hawaii": "Pacific/Honolulu",
    "Louisville (USA)": "America/Kentucky/Louisville",
    "California": "America/Los_Angeles",
    "Nevada": "America/Los_Angeles",
    "Portland (USA)": "America/Los_Angeles",
    "Washington": "America/Los_Angeles",
    "District of Columbia": "America/New_York",
    "Florida": "America/New_York",
    "Kentucky": "America/New_York",
    "Maryland": "America/New_York",
    "Massachusetts": "America/New_York",
    "New Jersey": "America/New_York",
    "New York": "America/New_York",
    "North Carolina": "America/New_York",
    "Ohio": "America/New_York",
    "Pennsylvania": "America/New_York",
    "Virginia": "America/New_York",
    "Arizona": "America/Phoenix",
}
def gettimezone(loc):
    """From most specific to least. Try city, then region, then country"""
    try:
        try:
            try:
                return zones[loc.name]
            except KeyError:
                return zones[loc.region]
        except KeyError:
            return zones[loc.country]
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

def codegenloc(loc):
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

def codegen(locs):
    t = time.localtime()
    t_s = "%s.%s.%s\n\n" % (t.tm_mday, t.tm_mon, t.tm_year)

    pre = "// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>\n"
    pre += "// Licensed under the GNU Public License, version 3.\n"
    pre += "//\n// Generated with %s on %s" % (sys.argv[0], t_s)
    pre += "using LibSolar.Types;\n\n"
    pre += "namespace LibSolar.Locations\n{\n"
    pre += "\tpublic class LocationListData\n\t{\n"
    pre += "\t\tpublic static LocationList GetStandardList()\n\t\t{\n"
    pre += "\t\t\tLocationList list = new LocationList();\n"

    post = "\t\t\treturn list;\n\t\t}\n"
    post += "\t}\n}"

    cd = pre
    for loc in locs:
        cd += codegenloc(loc) + "\n"
    cd += post

    return cd

def getspeciallocs():
    locs = []
    locs.append(Location(None, "Equator", [], "locality", 0, "0", "0", "", ""))
    locs.append(Location(None, "North Pole", [], "locality", 0, "9000", "0", "", ""))
    locs.append(Location(None, "South Pole", [], "locality", 0, "-9000", "0", "", ""))
    return locs


if __name__ == "__main__":
    try:
        datafile = sys.argv[1]
    except IndexError:
        print "Usage:  %s <datafile>" % sys.argv[0]
        sys.exit()
    
    # read locations
    lines = read(datafile)
    locs = compile(lines)

    # do some filtering
    locs = filtercat(locs, ["locality"])
    locs = sortpop(locs)
    locs = killdupes(locs)
    locs = filtermissingpos(locs)

    # 2000 world, 100 Norway
    final_locs = filternotcountry(locs, "Norway")[:2000]
    nor_locs = filtercountry(locs, "Norway")[:100]
    final_locs.extend(nor_locs)
    final_locs.extend(getspeciallocs())

    for loc in final_locs: sys.stderr.write("%s\n" % loc)
    final_locs = sortname(final_locs)
#    final_locs = sortcountry(final_locs)
    print(codegen(final_locs))
#    for loc in final_locs: sys.stderr.write("%s\n" % loc)
