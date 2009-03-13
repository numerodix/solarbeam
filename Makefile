all:
	mdtool build --configuration:Release

zip:
	rm -rf dist
	mkdir -p dist/solarbeam
	for d in `find . -type d -iname "Release"`; do cp $$d/* dist/solarbeam; done
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
	rm -rf ~/t/solarbeam/ /ex/solarbeam && cp -ar ~/code/solarbeam/ ~/t && cp -ar ~/code/solarbeam/ /ex/solarbeam

lines:
	find libsolar console solarbeam  -iname '*.cs' | xargs cat | wc -l
	find ref -iname '*.py' | xargs cat | wc -l
