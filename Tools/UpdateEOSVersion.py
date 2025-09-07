
# HOW TO USE THIS:

# Move the EOS SDK Download to the "Tools" directory
# and rename it to EOSUpdate.zip. Be sure to do this while
# not running Unity or while Unity is unfocused so it doesn't
# generate .meta files.

# Then, run the script from any command line with "py UpdateEOSVersion.py"
# and then delete the leftover files/folders in the "Tools" directory: 
# EOSUpdate.zip, the unarchived contents, the old files
# which have been placed into the "OldSDKFiles" folder, and 
# the unneeded files placed into the "UnusedSDKUpdateFiles" folder.

# If everything went well, you will see "Update complete!" printed.
# After the update is complete, don't forget to set the platform
# checkboxes for all of the .dll files in the Plugins folder.

import zipfile
import shutil
import os
from pathlib import Path

sdkFolder = "../Runtime/EOSSDK"
sdkZipFileName = "EOSUpdate.zip"
sdkUnzippedFolderName = "EOSUpdateExtracted"
oldSdkJunkFolderName = "OldSDKFiles"
unusedSdkJunkFolderName = "UnusedSDKUpdateFiles"

def unzip_file(zip_file_path : str, extract_to : str):
	with zipfile.ZipFile(zip_file_path, 'r') as zip_ref:
		zip_ref.extractall(extract_to)
	print(f"Extracted {zip_file_path} to {extract_to}")

def copy_file(source_file : str, destination_file : str):
	source = Path(source_file)
	dest = Path(destination_file)
	
	if(not source.exists()):
		print(f"Error: Source path {source_file} did not exist!")
		return
	
	try:
		shutil.copy2(source, dest)
		print(f"- Copied {source} to {dest}")
	except Exception as e:
		print(f"Error: Exception when copying file!")
		print(e)

def copy_folder_tree(src_folder : str, dst_folder : str):
	source = Path(src_folder)
	dest = Path(dst_folder)

	if(not source.exists()):
		print(f"Error: Source path {source} did not exist!")
		return

	try:
		shutil.copytree(source, dest, False, None)
		print(f"- Copied Folder Tree {src_folder} to {dst_folder}")
	except Exception as e:
		print(f"Error: Exception when copying folder tree!")
		print(e)

def copy_all(src : str, dst : str):
	if(os.path.isdir(src)):
		copy_folder_tree(src, dst)
	else:
		copy_file(src, dst)

def move_file(src : str, dst : str):
	source = Path(src)
	dest = Path(dst)

	if(not source.exists()):
		print(f"Error: Source path {src} did not exist!")
		return
	
	try:
		shutil.move(source, dest)
		print(f"- Moved {source} to {dest}")
	except Exception as e:
		print(f"Error: Exception when moving file! {source} to {dest}")
		print(e)

def move_file_to_junk(src : str, specifiedJunkFolder : str):
	sourcePath = Path(src)
	if(not sourcePath.exists()):
		print(f"- No file {src} found to move to {specifiedJunkFolder}, skipping...")
		return

	junkPath = Path(specifiedJunkFolder)
	if(not junkPath.exists()):
		os.makedirs(junkPath)

	if(not junkPath.exists()):
		print(f"Error: JunkPath {specifiedJunkFolder} was not created correctly")
		return

	backRemovedPath : str = src
	if(backRemovedPath.startswith("../")):
		backRemovedPath = backRemovedPath.removeprefix("../")

	move_file(src, os.path.join(junkPath, backRemovedPath))

def move_unity_file_to_junk(src : str, specifiedJunkFolder : str):
	move_file_to_junk(src, specifiedJunkFolder)
	move_file_to_junk(src + ".meta", specifiedJunkFolder)

def _main():
	print(f"Starting EOS Updater...")
	print(f"Be sure to move the SDK zip to this directory & name it {sdkZipFileName}")
	
	# Unzip the file
	pathToZipFileString = sdkZipFileName
	zipFilePath = Path(pathToZipFileString)
	if(not zipFilePath.is_file()):
		# Zip file doesn't exist
		print(f"Error: No {sdkZipFileName} found in same directory")
		return
	
	pathToUnzipFolderString = sdkUnzippedFolderName
	unzip_file(pathToZipFileString, pathToUnzipFolderString)
	
	extractedFolderPath = Path(pathToUnzipFolderString)
	if(not extractedFolderPath.is_dir()):
		# Folder doesn't exist
		print(f"Error: No folder {sdkUnzippedFolderName} found in same directory")
		return

	# Now move the current SDK files into a different folder
	# so they can be deleted easily from File Explorer. 
	# This is safer than trying to "rm"
	# from Python just in case something goes wrong.

	pathToSDKParent = os.path.join(sdkFolder, "SDK")
	pathToThirdPartyNotices = os.path.join(sdkFolder, "ThirdPartyNotices")

	move_unity_file_to_junk(pathToSDKParent, oldSdkJunkFolderName)
	move_unity_file_to_junk(pathToThirdPartyNotices, oldSdkJunkFolderName)

	# Now copy the extracted SDK files to the EOSSDK folder

	pathToExtractedSDKFolder = os.path.join(extractedFolderPath, "SDK")
	pathToExtractedThirdPartyFolder = os.path.join(extractedFolderPath, "ThirdPartyNotices")

	copy_all(pathToExtractedSDKFolder, pathToSDKParent)
	copy_all(pathToExtractedThirdPartyFolder, pathToThirdPartyNotices)

	# Rename "Bin" to "Plugins"

	pathToBinFolder = os.path.join(pathToSDKParent, "Bin")
	pathToPluginsFolder = os.path.join(pathToSDKParent, "Plugins")
	move_file(pathToBinFolder, pathToPluginsFolder)

	# Then systematically move unneeded files and folders to junk,
	# such as the "SDK/Tools" folder and IOS/Android framework

	move_unity_file_to_junk(os.path.join(pathToSDKParent, "Tools"), unusedSdkJunkFolderName)
	move_unity_file_to_junk(os.path.join(pathToPluginsFolder, "Android"), unusedSdkJunkFolderName)
	move_unity_file_to_junk(os.path.join(pathToPluginsFolder, "IOS"), unusedSdkJunkFolderName)
	move_unity_file_to_junk(os.path.join(pathToPluginsFolder, "libEOSSDK-LinuxArm64-Shipping.so"), unusedSdkJunkFolderName)
	
	# All steps are now done!

	print(f"Update complete!")
	

_main()

