#!/usr/bin/env bash
#
#
# AN IMPORTANT THING: FOR THIS SAMPLE YOU NEED DECLARE THE SPECIFIED ENVIRONMENT VARIABLES IN APP CENTER BUILD CONFIGURATION.

echo "Running pre-build script for Mobile NPC"

if [ -z "$AKENEO_URL" ]
then
    echo "You need define the AKENEO_URL variable in App Center"
    exit
fi
if [ -z "$AKENEO_USERNAME" ]
then
    echo "You need define the AKENEO_USERNAME variable in App Center"
    exit
fi
if [ -z "$AKENEO_PASSWORD" ]
then
    echo "You need define the AKENEO_PASSWORD variable in App Center"
    exit
fi
if [ -z "$AKENEO_CLIENT_ID" ]
then
    echo "You need define the AKENEO_CLIENT_ID variable in App Center"
    exit
fi
if [ -z "$AKENEO_CLIENT_SECRET" ]
then
    echo "You need define the AKENEO_CLIENT_SECRET variable in App Center"
    exit
fi
if [ -z "$AKENEO_CONFIG_URL" ]
then
    echo "You need define the AKENEO_CONFIG_URL variable in App Center"
    exit
fi

APP_CONSTANT_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileNPC/Core/AppConstants.cs

if [ -e "$APP_CONSTANT_FILE" ]
then
    echo "Updating ApiUrl to $AKENEO_URL in AppConstant.cs"
    sed -i '' 's#AkeneoUrl = "[-A-Za-z0-9:_./]*"#AkeneoUrl = "'$AKENEO_URL'"#' $APP_CONSTANT_FILE
    echo "Updating Username to $AKENEO_USERNAME in AppConstant.cs"
    sed -i '' 's#Username = "[-A-Za-z0-9:_./]*"#Username = "'$AKENEO_USERNAME'"#' $APP_CONSTANT_FILE
    echo "Updating Password to $AKENEO_PASSWORD in AppConstant.cs"
    sed -i '' 's#Password = "[-A-Za-z0-9:_./]*"#Password = "'$AKENEO_PASSWORD'"#' $APP_CONSTANT_FILE
    echo "Updating ClientId to $AKENEO_CLIENT_ID in AppConstant.cs"
    sed -i '' 's#ClientId = "[-A-Za-z0-9:_./]*"#ClientId = "'$AKENEO_CLIENT_ID'"#' $APP_CONSTANT_FILE
    echo "Updating ClientSecret to $AKENEO_CLIENT_SECRET in AppConstant.cs"
    sed -i '' 's#ClientSecret = "[-A-Za-z0-9:_./]*"#ClientSecret = "'$AKENEO_CLIENT_SECRET'"#' $APP_CONSTANT_FILE
    echo "Updating AkeneoConfigUrl to $AKENEO_CONFIG_URL in AppConstant.cs"
    sed -i '' 's#AkeneoConfigUrl = "[-A-Za-z0-9:_./]*"#AkeneoConfigUrl = "'$AKENEO_CONFIG_URL'"#' $APP_CONSTANT_FILE

    echo "File content:"
    cat $APP_CONSTANT_FILE
fi

