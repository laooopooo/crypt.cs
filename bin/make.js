#!/usr/bin/env node
/* global cd, config, cp, echo, exec, rm, target */

/**
 * Build system.
 * @module bin.make
 */
'use strict';

// Module dependencies.
require('shelljs/make');

/**
 * Provides tasks for [ShellJS](http://shelljs.org) make tool.
 * @class cli.Makefile
 * @static
 */
cd(__dirname+'/..');

/**
 * The application settings.
 * @property config
 * @type Object
 */
config.fatal=true;

/**
 * Runs the default tasks.
 * @method all
 */
target.all=function() {
  echo('Please specify a target. Available targets:');
  for(var task in target) {
    if(task!='all') echo(' ', task);
  }
};

/**
 * Deletes all generated files and reset any saved state.
 * @method clean
 */
target.clean=function() {
  echo('Delete the output files...');
  rm('-rf', 'bin/debug');
  rm('-rf', 'bin/release');
  rm('-rf', 'src/crypt.console/obj');
  rm('-rf', 'src/crypt.core/obj');
  rm('-rf', 'src/crypt.encoders/obj');
  rm('-rf', 'src/crypt.windows/obj');
};

/**
 * Builds the documentation.
 * @method doc
 */
target.doc=function() {
  echo('Build the documentation...');
  exec('docgen');
  cp('-f', [ 'www/apple-touch-icon.png', 'www/favicon.ico' ], 'doc/api/assets');
};

/**
 * Performs static analysis of source code.
 * @method lint
 */
target.lint=function() {
  config.fatal=false;

  echo('Static analysis of JavaScript sources...');
  exec('jshint --verbose bin');

  echo('Static analysis of documentation comments...');
  exec('docgen --lint');

  config.fatal=true;
};
