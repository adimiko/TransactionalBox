#!/bin/bash
chmod +x ./.github/scripts/deploy-package.sh

echo Started deploy.

for dir in source/*/
do
    dir=${dir%*/}
    
    echo Deploying package:  ${dir##*/}

    exec ./scripts/publish-package.sh  ${dir##*/} &
    wait
done

echo Finished deploy.