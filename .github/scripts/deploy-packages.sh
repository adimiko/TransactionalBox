#!/bin/bash
chmod +x ./.github/scripts/deploy-package.sh

echo Started deploy.

for dir in source/*/
do
    dir=${dir%*/}
    
    if [[ ${dir##*/} == 'Internals' ]]
	then
		continue
	fi

    echo Deploying package:  ${dir##*/}

    exec ./.github/scripts/deploy-package.sh  ${dir##*/} &
    wait
done

echo Finished deploy.