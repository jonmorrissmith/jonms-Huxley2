# Huxley 2 for Raspberry Pi

A fork of [James Singleton's Huxley2](https://github.com/jpsingleton/Huxley2) repository with changes to run locally on a Raspberry Pi.

**Important** Refer to [Huxley2](https://github.com/jpsingleton/Huxley2) for usage, licensing and updates.

The list of customisations here is in [Pi Customisations](https://github.com/jonmorrissmith/jonms-Huxley2/blob/master/Pi_Customisations.md)

# Get access to the train data #

The source of the data is the [Live Departure Boards Web Service (LDBWS / OpenLDBWS)](https://lite.realtime.nationalrail.co.uk/OpenLDBWS/)

You'll need to register to be able to access - this is pretty straightforward via the [Open LDBWS Registration page](https://realtime.nationalrail.co.uk/OpenLDBWSRegistration).  

Make a note of the key (although they email it to you aswell).

# Raspberry Pi installation #

## Install the OS ##

There are more tutorials than you can shake a stick at on how to install an OS on a Raspberry Pi.

I went for the '**OS Lite (64bit)**' to maximise CPU cycles by having a cut-down OS. 

Set up ssh and Wifi in the Raspberry Pi Imager tool.

Once installed there was the usual upgrade/update and disable/uninstall anything unecessary.

This installation was tested assuming you're using this for your [Raspberry Pi RGB Departure Board](https://github.com/jonmorrissmith/RGB_Matrix_Train_Departure_Board).

The _Raspberry Pi RGB Departure Board_ repo has information for removing unecessary packages to improve performance.

## Install the dependencies ###

### Install Docker ###

The default docker install is somewhat crufty - follow the [Debian Docker install instructions](https://docs.docker.com/engine/install/debian/).

I'll not reproduce those here as they may well change over time.

**Important** Don't forget the [post-installation steps](https://docs.docker.com/engine/install/linux-postinstall/)

### Install .Net 8 ###

Refer to the [Microsoft installation instructions](https://learn.microsoft.com/en-us/dotnet/iot/deployment)

At the time of writing (Feb 2025) the latest version of the .Net 8 SDK is 8.0.402 according to [.Net 8 release notes](https://github.com/dotnet/core/blob/main/release-notes/8.0/README.md)

A dry-run first is always a good idea.
```
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.402 --verbose --dry-run
```
Then run the install...
```
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.402 --verbose
```
... and configure your environment
```
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc
```
### Useful tools ###

Useful for handling JSON output
```
sudo apt install jq
```
# Running Huxley2 #

Create an `.env` file in the `Huxley2` directory with the access tokens. You can delete the ones you're not using.

Chances are you'll just need the ACCESS_TOKEN - see [Huxley2](https://github.com/jpsingleton/Huxley2) for more detail. 

Example `.env` file:
```env
ACCESS_TOKEN=abcde12345
STAFF_ACCESS_TOKEN=abcde12345
CLIENT_ACCESS_TOKEN=abcde12345
```
Build using `docker compose up` - this may take a while.

Once completed you should be able to access via `localhost:8081` - for example:
```
curl localhost:8081/departures/bhm | jq
```

To rebuild use `docker-compose build` or `docker-compose up --build`.

If you want to run the container in the background you can run `docker-compose up --detach`

If you would like the docker container to _reboot upon restart_ on the host machine you can uncomment `restart: always` in the docker-compose.yml file and make sure the docker service is set to start upon bootup.

# Huxley2 and the RGB Matrix Train Departure Board #

You can run Huxley2 on the same Raspberry Pi as the [RGB Matrix Train Departure Board](https://github.com/jonmorrissmith/RGB_Matrix_Train_Departure_Board).

Set the APIURL to `http://localhost:8081` in the configuration file or via the UI

The increased load of running both may introduce some flickering when the API is queried for updates.

Tweaking parameters may alleviate this, or you could run Huxley2 on a separate Raspberry Pi.
