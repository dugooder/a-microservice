var gulp = require( "gulp" ),
    msbuild = require( "gulp-msbuild" ),
    del = require( "del" ),
    args = require( "yargs" ).argv,
    assemblyInfo = require( "gulp-dotnet-assembly-info" ),
    shell = require( "gulp-shell" ),
    fs = require( "fs" ),
    request = require( "request" ),
    xunit = require( "gulp-xunit-runner" ),
    flatten = require( "gulp-flatten" ),
    runseq = require( "gulp-run-sequence" ),
    colors = require( "colors/safe" );

var projects = [
    "lib.io", "lib.logging",
    "lib.repos", "lib.repos.common",
    "service.health", "service.home",
    "service.host"
    ];

var projectsWithTests = [
    "service.health", "service.home",
    "lib.logging", "lib.repos.common",
    "lib.repos", "lib.repos.common", "lib.repos"
    ];

var pkg = require( "./package.json" ),
    srcDir = "src/",
    buildFolder = "build/",
    testsSuffix = ".tests",
    configuration = args.configuration || "Release",
    nugetDownloadURL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe",
    xunitConsoleExe =  "src/packages/xunit.runner.console.2.1.0/tools/xunit.console.exe";

gulp.task( "nuget-download", function( done ) {
    if ( !fs.existsSync( "nuget.exe" ) ) {
        return request( nugetDownloadURL )
               .pipe( fs.createWriteStream( "nuget.exe" ) );
    } else {
        return done();
    }
} );

gulp.task( "nuget-restore", [ "nuget-download" ],
     shell.task( [ "nuget restore src/" + pkg.name + ".sln " ] ) );

gulp.task( "info", function() {
    console.log( "Building version:" + pkg.version );
    console.log( "Configuration:" + configuration );
} );

gulp.task( "assemblyInfo", function() {
    return gulp
        .src( "**/AssemblyInfo.cs" )
        .pipe( assemblyInfo( {
            title: pkg.title,
            description: pkg.description,
            configuration: configuration,
            company: pkg.company,
            product: pkg.product,
            trademark: pkg.trademark,
            culture: pkg.culture,
            version: pkg.version,
            fileVersion: pkg.version,
            copyright: function() {
                return pkg.company + " " + new Date().getFullYear();
            }
        } ) )
        .pipe( gulp.dest( "." ) );
} );

gulp.task( "clean", function() {
    return del( [ "src/**/bin/**/*",
                    "src/**/obj/**/*",
                    buildFolder + "*" ],
        { force: true } );
} );

gulp.task( "compile", function() {
    if ( !fs.existsSync( buildFolder ) ) {
        fs.mkdirSync( buildFolder );
    }

    return gulp
        .src( "**/" + pkg.name + ".sln" )
        .pipe( msbuild( {
            toolsVersion: 14.0,
            targets: [ "Rebuild" ],
            errorOnFail: true,
            stdout: true,
            fileLoggerParameters: "LogFile=" + buildFolder + "Build.log;Verbosity=Normal",
            properties: { Configuration: configuration }
        } ) );
} );

gulp.task( "test", [ "compile" ], function() {
    var testsDllDir = [];

    for ( var i = 0; i < projectsWithTests.length; i++ ) {
        var p = projectsWithTests[ i ];
        testsDllDir.push( srcDir + p + testsSuffix +
            "/bin/" + configuration + "/*" +
            p + testsSuffix + ".dll" );
    }

    console.log( testsDllDir );

    return gulp.src( testsDllDir, { read: false } )
        .pipe( xunit( {
                executable: xunitConsoleExe,
                options: {
                    nologo: true,
                    html: buildFolder + "test_results.html"
                }
              } )
            );
} );

gulp.task( "package", function() {
    var projectsToPack = [];

    for ( var i = 0; i < projects.length; i++ ) {
        var p = projects[ i ];
        projectsToPack.push( srcDir + p +
            "/bin/" + configuration +
            "/**/*.*" );
    }

    projectsToPack.push( "!/**/*.dll.config" );
    console.log( projectsToPack );

    return gulp.src( projectsToPack )
        .pipe( gulp.dest( buildFolder ) );
} );

gulp.task( "build", function( callback ) {
    runseq( "info",
        "clean",
        "nuget-restore",
        "assemblyInfo",
        "compile",
        "test",
        "package",
        "info",
        callback );
} );
