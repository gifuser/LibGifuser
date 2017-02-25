# Overview

This repository provides the core functionality for the Gifuser application such as:

* Recording gifs asynchronously
* Uploading gifs asynchronously to file services with upload progress
    * [imgur](https://imgur.com)
    * [giphy](https://giphy.com)
    * [gfycat](https://gfycat.com)

## Projects

* Core
    * [LibGifuser](#libgifuser)
    * [Gifuser.Core](#gifusercore)
    * [Gifuser.Upload](#gifuserupload)
* Plugins
    * [ImgurPlugin](#imgurplugin)
    * [GiphyPlugin](#giphyplugin)
    * [GfycatPlugin](#gfycatplugin)

### LibGifuser

This C++ project provides the core functionality to capture the user's screen
and create the underlying gif file.

### Gifuser.Core

This project interoperates with the *LibGifuser* project above
and adds the feature to record screen asynchronously on a separate thread. This means
that the UI thread remains free to handle user input.

### Gifuser.Upload

This project provides a base to upload files asynchronously to external services
with upload progress, when possible.

### ImgurPlugin

This project extends *Gifuser.Upload* and uploads files to [imgur](https://imgur.com),
tracking upload progress.

### GiphyPlugin

This project extends *Gifuser.Upload* and uploads files to [giphy](https://giphy.com),
tracking upload progress.

### GfycatPlugin

This project extends *Gifuser.Upload* and uploads files to [gfycat](https://gfycat.com),
but does not track upload progress.

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
3. Download sources, download nuget, restore solution and build everything (*pay attention to replace each path in the make line below*)

   ```
   git clone https://github.com/gifuser/LibGifuser.git
   cd LibGifuser
   wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
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
4. If everything ran without issues, you should see a Release directory containing
several .dll files. After that, you need to copy Newtonsoft.Json.dll into this Release folder, which can be found at

   ```
   ./packages/Newtonsoft.Json.9.0.1/lib/net45/Newtonsoft.Json.dll
   ```
   **Remark**: ocasionally, 9.0.1 in the path above may change due newer releases
   of Newtonsoft.Json library

## Windows

The preferred way to build these projects is using a recent version of Visual Studio with support
for both C++ Win32 and C#. Then, build the projects through the IDE

1. Download and install a recent version of Visual Studio
2. Open Visual Studio, restore solution and build the project **LibGifuser**
    
    **Remark**: if you are building the C++ project LibGifuser with a toolset other than
    MSVC, you need to pass *user32.lib* and *gdi.lib* to the linker.
3. Build remaining projects
4. If everything ran without issues, you should see a Release directory containing
several .dll files. After that, you need to copy Newtonsoft.Json.dll into this Release folder, which can be found at

   ```
   .\packages\Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll
   ```
   **Remark**: ocasionally, 9.0.1 in the path above may change due newer releases
   of Newtonsoft.Json library
