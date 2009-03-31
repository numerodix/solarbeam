#!/usr/bin/perl
# Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
# Licensed under the GNU Public License, version 3.
#
# Build/update mono from svn

use warnings;

use Cwd;
use File::Path;
use Term::ReadKey;


my $SRCDIR = "/ex/mono-sources";
my $DESTDIR = "/ex/mono";


sub term_title {
	my ($s) = @_;
	system("echo", "-en", "\033]2;$s\007");
}

sub invoke {
	my (@args) = @_;

	print "> "; foreach my $a (@args) { print "$a "; }; print "\n";

	$exit = system(@args);
	return $exit;
}

sub dopause {
	ReadMode 'cbreak';
	ReadKey(0);
	ReadMode 'normal';
}


sub env_var {
	my ($var) = @_;
	my ($val) = $ENV{$var};
	return defined($val) ? $val : "";
}

sub env_get {
	my ($env) = {
		DYLD_LIBRARY_PATH => "$DESTDIR/lib:" . env_var("DYLD_LIBRARY_PATH"),
		LD_LIBRARY_PATH => "$DESTDIR/lib:" . env_var("LD_LIBRARY_PATH"),
		C_INCLUDE_PATH => "$DESTDIR/include:" . env_var("C_INCLUDE_PATH"),
		ACLOCAL_PATH => "$DESTDIR/share/aclocal",
		PKG_CONFIG_PATH => "$DESTDIR/lib/pkgconfig",
		XDG_DATA_HOME => "$DESTDIR/share:" . env_var("XDG_DATA_HOME"),
		XDG_DATA_DIRS => "$DESTDIR/share:" . env_var("XDG_DATA_DIRS"),
		PATH => "$DESTDIR/bin:$DESTDIR:" . env_var("PATH"),
		PS1 => "[mono] \\w \\\$? @ ",
	};
	return $env;
}

sub env_set {
	my ($env) = env_get();
	foreach my $key (keys %$env) {
		if ((!exists($ENV{$key})) || ($ENV{$key} ne $env->{$key})) {
			$ENV{$key} = $env->{$key};
		}
	}
}

sub env_write {
	my ($env) = env_get();
	open (WRITE, ">", "env.sh");
	foreach my $key (keys %$env) {
		my ($line) = sprintf("export %s=\"%s\"\n", $key, $env->{$key});
		print(WRITE $line);
	}
	close(WRITE);
}


sub pkg_get {
	my ($name, $svnurl) = @_;
	my $pkg = {
		name => $name,
		dir => $name,
		svnurl => $svnurl,
		configurer => "configure",
		maker => "make",
		installer => "make install",
	};
	return $pkg;
}

sub pkg_print {
	my ($pkg) = @_;
	foreach my $key (keys %$pkg) {
		printf("%14s : %s\n", $key, $pkg->{$key});		
	}
	print("\n");
}

sub pkg_action {
	my ($action, $pkg, $code) = @_;

	term_title(sprintf("Running %s %s", $action, $pkg->{name}));

	my ($path) = File::Spec->catdir($SRCDIR, $pkg->{dir});
	unless (-d $pkg->{dir}) {
		mkpath($path);
	}

	my ($cwd) = getcwd();
	chdir($path);

	env_set();
	my ($exit) = &$code;

	chdir($cwd);

	if ($exit == 0) {
		term_title(sprintf("Done %s %s", $action, $pkg->{name}));
	} else {
		term_title(sprintf("Failed %s %s", $action, $pkg->{name}));
		dopause();
	}
}

sub pkg_fetch {
	my ($pkg, $rev) = @_;
	
	if (exists($pkg->{svnurl})) {
		my $code = sub {
			return invoke("svn", "checkout", "-r", $rev, $pkg->{svnurl}, ".");
		};
		pkg_action("fetch", $pkg, $code);
	}
}

sub pkg_configure {
	my ($pkg) = @_;

	if (exists($pkg->{configurer})) {
		my $code = sub {
			my ($configurer) = $pkg->{configurer};
			if (!-e $configurer) {
				if (-e "autogen.sh") {
					$configurer = "autogen.sh";
				}
			}
			return invoke("./$configurer --prefix=$DESTDIR");
		};
		pkg_action("configure", $pkg, $code);
	}
}

sub pkg_make {
	my ($pkg) = @_;

	if (exists($pkg->{maker})) {
		my $code = sub {
			return invoke($pkg->{maker});
		};
		pkg_action("make", $pkg, $code);
	}
}

sub pkg_install {
	my ($pkg) = @_;

	if (exists($pkg->{installer})) {
		my $code = sub {
			return invoke($pkg->{installer});
		};
		pkg_action("install", $pkg, $code);
	}
}


env_write();

my $mono_svn = "svn://anonsvn.mono-project.com/source/trunk";
my (@pkgs) = ( 
#	{"libgdiplus" => "$mono_svn/libgdiplus"}, 
#	{"mcs" => "$mono_svn/mcs"}, 
#	{"mono" => "$mono_svn/mono"}, 
#	{"debugger" => "$mono_svn/debugger"}, 
#	{"mono-addins" => "$mono_svn/mono-addins"}, 
#	{"mono-tools" => "$mono_svn/mono-tools"}, 
#	{"gtk-sharp" => "$mono_svn/gtk-sharp"}, 
#	{"gnome-sharp" => "$mono_svn/gnome-sharp"}, 
#	{"monodoc-widgets" => "$mono_svn/monodoc-widgets"}, 
#	{"monodevelop" => "$mono_svn/monodevelop"}, 
#	{"olive" => "$mono_svn/olive"}, 
#	{"paint-mono" => "http://paint-mono.googlecode.com/svn"},
);

foreach my $pkgh (@pkgs) {

	# prep
	my @ks = keys(%$pkgh);
	my $key = $ks[0];

	# init pkg
	my $pkg = pkg_get($key, $pkgh->{$key});

	# overwrite defaults
	if ($pkg->{name} eq "mcs") {
		delete($pkg->{configurer});
		delete($pkg->{maker});
		delete($pkg->{installer});
	}
	if ($pkg->{name} eq "gtk-sharp") {
		$pkg->{configurer} = "bootstrap-2.4";
	}
	if ($pkg->{name} eq "gnome-sharp") {
		$pkg->{configurer} = "bootstrap-2.24";
	}

	pkg_print($pkg);

	# fetch
	my $revision = "HEAD";
	pkg_fetch($pkg, $revision);

	# configure
	pkg_configure($pkg);

	# make
	pkg_make($pkg);

	# install
	pkg_install($pkg);
}

