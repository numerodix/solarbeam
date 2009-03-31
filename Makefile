all:
	mdtool build --configuration:Release

png:
	for f in icons/*.svg; do g=`echo $$f | sed "s/.svg//g"`; convert -quantize RGB -resize 20x20 $$f $$g.png; done
	#for f in icons/*.svg; do g=`echo $$f | sed "s/.svg//g"`; inkscape $$f --export-png=$$g.png --export-width=20 --export-height=20; done

zip:
	rm -rf dist
	mkdir -p dist/solarbeam
	for d in `find . -type d -iname "Release"`; do cp $$d/* dist/solarbeam; done
	rm locations.bin; ./gui -nogui; cp locations.bin dist/solarbeam
	cp zoneinfo dist/solarbeam
	cd dist
	zip -j dist/solarbeam.zip dist/solarbeam/*
	rm -rf dist/solarbeam

apidocs:
	rm -rf apidoc
	doxygen doxygen.cfg

test:
	nunit-console libsolar/bin/Release/libsolar.dll
	rm TestResult.xml "C:\NUnitPrimaryTrace.txt"

arch:
	rm -rf apidoc
	rm -rf ~/t/solarbeam/ /ex/solarbeam && cp -ar ~/code/solarbeam/ ~/t && cp -ar ~/code/solarbeam/ /ex/solarbeam

darch:
	mdtool build --configuration:Debug
	rm -rf apidoc
	rm -rf ~/t/solarbeam/ /ex/solarbeam && cp -ar ~/code/solarbeam/ ~/t && cp -ar ~/code/solarbeam/ /ex/solarbeam

lines:
	@echo -n " * Codebase     :  "
	@find -iname '*.cs' -o -iname '*.py' -o -iname '*.pl' -o -iname '*.js' -o -iname '*.html' | xargs cat | wc -l
	@echo -n " * C#           :  "
	@find -iname '*.cs' | xargs cat | wc -l
	@echo -n " *   original   :  "
	@find libsolar console solarbeam  -iname '*.cs' | xargs cat | wc -l
	@echo -n " *   libs       :  "
	@find libpublicdomain -iname '*.cs' | xargs cat | wc -l
	@echo -n " * Python       :  "
	@find -iname '*.py' | xargs cat | wc -l
	@echo -n " * Perl         :  "
	@find -iname '*.pl' | xargs cat | wc -l
	@echo -n " * Javascript   :  "
	@find -iname '*.js' | xargs cat | wc -l
	@echo -n " *   libs       :  "
	@find -iname '*.js' | xargs cat | wc -l
	@echo -n " * HTML         :  "
	@find -iname '*.html' | xargs cat | wc -l
	@echo -n " *   libs       :  "
	@find -iname '*.html' | xargs cat | wc -l
