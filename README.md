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
2. Install the latest version of [Mono](http://www.mono-project.com). Then, also install the package
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
   xbuild LibGifuser.sln /p:Configuration=Release
   ```   
   Parameters in the ```make``` line above:
      * **CGF**: either *Debug* or *Release*
      * **XLIB_INC**: directory containing X11 include directory
      * **XLIB_LIB**: directory containing *libX11.so* file
      * **XFIXES_LIB**: directory containing *libXfixes.so* file
4. If everything ran without issues (don't care for warnings), you should see a Release directory containing
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
2. Download LibGifuser sources, open it inside Visual Studio and restore solution
3. Build the C++ project LibGifuser
    
    **Remark**: if you are building the project with a toolset other than
    MSVC, you need to pass *user32.lib* and *gdi32.lib* to the linker.
4. Build remaining projects
5. If everything ran without issues (don't care for warnings), you should see a Release directory (if you chosen Release configuration) containing
several .dll files. After that, you need to copy Newtonsoft.Json.dll into this Release folder, which can be found at

   ```
   .\packages\Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll
   ```
   **Remark**: ocasionally, 9.0.1 in the path above may change due newer releases
   of Newtonsoft.Json library
