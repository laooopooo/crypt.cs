#!/bin/sh
# Builds the documentation.

basedir=`dirname "$0"`
cd "$basedir/.."

doxygen doc/api.doxyfile
cp --force web/favicon.ico doc/api
