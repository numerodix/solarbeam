all:
	mdtool build --configuration:Release

png:
	for f in icons/*.svg; do g=`echo $$f | sed "s/.svg//g"`; convert -quantize RGB -resize 20x20 $$f $$g.png; done
	#for f in icons/*.svg; do g=`echo $$f | sed "s/.svg//g"`; inkscape $$f --export-png=$$g.png --export-width=20 --export-height=20; done

zip:
	rm -rf dist
	mkdir -p dist/solarbeam
	for d in `find . -type d -iname "Release"`; do cp $$d/* dist/solarbeam; done
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
	find -iname '*.cs' | xargs cat | wc -l
	find libsolar console solarbeam  -iname '*.cs' | xargs cat | wc -l
	find -iname '*.py' | xargs cat | wc -l
	find -iname '*.js' | xargs cat | wc -l
	find -iname '*.html' | xargs cat | wc -l
