# Vortex Worlds

This is a mini mod to help with installing world saves to Valheim, using the Nexus Mods Vortex mod manager.  

It is included with the default Valheim integration that Vortex installs for you, so you do not need to do anything.    

It works by deploying world saves that it finds in the `vortex-worlds` directory, to Valheim's `worlds` directory, ***if a world with the same name doesn't already exist***.   

It will **NEVER** overwrite an existing file.  

The `vortex-worlds` directory should be next to the `worlds` directory. If it doesn't exist, then this mod does nothing at all.

## How to package your worlds

You can find the `worlds` directory by pasting `%AppData%\..\LocalLow\IronGate\Valheim\worlds` into the Windows explorer nav bar.

In there you should see all your local worlds, each one with a set of files with different extensions.  

Simply zip up the files for your world. The easiest way to do this, is to just select the files you want to zip (hold the Ctrl key to multi-select in explorer), then right click on them and go to "Send to", and select "Compressed (zipped) folder":
![image](https://user-images.githubusercontent.com/1453936/114048960-eb8d2b80-9882-11eb-9462-896c9e628416.png)


For example if your world is called "Narnia", then you would zip up:

`Narnia.fwl` -- this is the vanilla world metadata file  
`Narnia.db` -- this is the vanilla database file

If you use some mods that generate extra files for the world save, you may or may not want to include these, depending on the implications of the mod (ask the mod author if you aren't sure).

### Some mod example files:  
`Narnia.fwl.BetterContinents` -- this is the [Better Continents](https://github.com/billw2012/BetterContinents) settings file and **should** be included with your world package.  
`Narnia.WeylandMod.SharedMap.dat` -- this is the [WeylandMod](https://github.com/WeylandMod/WeylandMod) shared player map, and **should not** be included in your world package.
