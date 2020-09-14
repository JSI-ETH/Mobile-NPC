#!/usr/bin/env bash
#

echo "Prebuild script running..."

if [ -z "$AKENEO_URL" ]
then
    echo "You need define the $AKENEO_URL variable in App Center"
    exit
fi
if [ -z "$AKENEO_USERNAME" ]
then
    echo "You need define the $AKENEO_USERNAME variable in App Center"
    exit
fi
if [ -z "$AKENEO_PASSWORD" ]
then
    echo "You need define the $AKENEO_PASSWORD variable in App Center"
    exit
fi
if [ -z "$AKENEO_CLIENT_ID" ]
then
    echo "You need define the $AKENEO_CLIENT_ID variable in App Center"
    exit
fi
if [ -z "$AKENEO_CLIENT_SECRET" ]
then
    echo "You need define the $AKENEO_CLIENT_SECRET variable in App Center"
    exit
fi
if [ -z "$AKENEO_CONFIG_URL" ]
then
    echo "You need define the $AKENEO_CONFIG_URL variable in App Center"
    exit
fi

APP_CONSTANT_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileNPC/Core/AppConstants.cs

echo "AppConstants file $APP_CONSTANT_FILE"

if [ -e "$APP_CONSTANT_FILE" ]
then
    echo "Updating Akeneo Configuration in AppConstants.cs"
    sed -i '' 's#AkeneoUrl = "[-A-Za-z0-9:_./]*"#AkeneoUrl = "'$AKENEO_URL'"#' $APP_CONSTANT_FILE
    sed -i '' 's#Username = "[-A-Za-z0-9:_./]*"#Username = "'$AKENEO_USERNAME'"#' $APP_CONSTANT_FILE
    sed -i '' 's#Password = "[-A-Za-z0-9:_./]*"#Password = "'$AKENEO_PASSWORD'"#' $APP_CONSTANT_FILE
    sed -i '' 's#ClientId = "[-A-Za-z0-9:_./]*"#ClientId = "'$AKENEO_CLIENT_ID'"#' $APP_CONSTANT_FILE
    sed -i '' 's#ClientSecret = "[-A-Za-z0-9:_./]*"#ClientSecret = "'$AKENEO_CLIENT_SECRET'"#' $APP_CONSTANT_FILE
    sed -i '' 's#AkeneoConfigUrl = "[-A-Za-z0-9:_./]*"#AkeneoConfigUrl = "'$AKENEO_CONFIG_URL'"#' $APP_CONSTANT_FILE

    echo "File content:"
    cat $APP_CONSTANT_FILE
fi
