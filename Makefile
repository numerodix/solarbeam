all:
	mdtool build --configuration:Release

zip:
	rm -rf dist
	mkdir -p dist/solarbeam
	for d in `find . -type d -iname "Release"`; do cp $$d/* dist/solarbeam; done
	(cd dist/solarbeam ; mono solarbeam.exe -init)
	cd ..
	zip -j dist/solarbeam.zip dist/solarbeam/*
	rm -rf dist/solarbeam

apidocs:
	rm -rf apidoc
	doxygen doxygen.cfg

test:
	nunit-console libsolar/bin/Release/libsolar.dll
	rm TestResult.xml #"C:\NUnitPrimaryTrace.txt"

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

webup:
	rsync -avP --delete -e ssh web/ numerodix,solarbeam@web.sourceforge.net:htdocs/
