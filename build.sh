#!/usr/bin/env bash
set -e
basepath=$(cd `dirname $0`; pwd)
artifacts=${basepath}/artifacts

if [[ -d ${artifacts} ]]; then
   rm -rf ${artifacts}
fi

mkdir -p ${artifacts}

dotnet restore src/IPSearcher
dotnet restore src/IPSearcher.Data

dotnet build src/IPSearcher -f net462 -c Release -o ${artifacts}/net462
dotnet build src/IPSearcher -f netstandard2.0 -c Release -o ${artifacts}/netstandard2.0

dotnet build src/IPSearcher.Data -f net462 -c Release -o ${artifacts}/net462
dotnet build src/IPSearcher.Data -f netstandard2.0 -c Release -o ${artifacts}/netstandard2.0

