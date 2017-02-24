# LibGifuser

This repository provides the core functionality for the Gifuser application such as:

* Recording gifs asynchronously
* Uploading gifs asynchronously to file services with upload progress
    * imgur
    * giphy
    * gfycat

# Build Instructions

The only *special* point worth to note before trying to build the libraries is:
build the C++ project **LibGifuser** as the first project.

## Linux

1. You need a C++ toolset like GCC and the libraries *X11* and *Xfixes* for development.
In Ubuntu, issuing this command you should be fine (might require admin privileges)

```
sudo apt-get install build-essential libx11-dev libxfixes-dev
```

2. Install the latest version of [Mono](http://www.mono-project.com) and the package
*mono-complete*

3. Install nuget

```
wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
```

4. Download sources, restore solution and build everything (*pay attention to replace each path in the make line below*)

```
git clone https://github.com/gifuser/LibGifuser.git
cd LibGifuser
mono nuget.exe restore LibGifuser.sln
cd LibGifuser
make CFG=Release XLIB_INC=/usr/include/ XLIB_LIB=/usr/lib/x86_64-linux-gnu/ XFIXES_LIB=/usr/lib/x86_64-linux-gnu/
cd ..
cd Gifuser.Core
xbuild Gifuser.Core.csproj /p:Configuration=Release
cd ..
cd Gifuser.Upload
xbuild Gifuser.Upload.csproj /p:Configuration=Release
cd ..
cd ImgurPlugin
xbuild ImgurPlugin.csproj /p:Configuration=Release
cd ..
cd GiphyPlugin
xbuild GiphyPlugin.csproj /p:Configuration=Release
cd ..
cd GfycatPlugin
xbuild GfycatPlugin.csproj /p:Configuration=Release
cd ..
```

5. If everything ran without issues, you should see a Release directory containing
several .dll files.

After that, you need to copy Newtonsoft.Json.dll into this Release folder, which can be found at
```
./packages/Newtonsoft.Json.9.0.1/lib/net45/Newtonsoft.Json.dll
```

## Windows

The preferred way to build these projects is using a recent version of Visual Studio with support
for both C++ Win32 and C#. Then, build the projects through the IDE

1. Download and install a recent version of Visual Studio
2. Open Visual Studio and build the project **LibGifuser**
3. Build the remaining projects
4. If everything ran without issues, you should see a Release directory containing
several .dll files.

After that, you need to copy Newtonsoft.Json.dll into this Release folder, which can be found at
```
.\packages\Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll
```

# Projects

## Core Projects

* LibGifuser (*build this project first*)
* Gifuser.Core
* Gifuser.Upload

## File Upload Plugin Projects

* ImgurPlugin
* GiphyPlugin
* GfycatPlugin