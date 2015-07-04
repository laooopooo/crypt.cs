#!/bin/sh

basedir=`dirname "$0"`
cd "$basedir/.."

doxygen docs/api.doxyfile
cp --force web/favicon.ico docs/api
