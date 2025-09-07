
#WARNING: Experimental! Don't use this yet!

# HOW TO USE THIS:

# Move the EOS SDK Download to the "Tools" directory
# and rename it to EOSUpdate.zip. Be sure to do this while
# not running Unity or while Unity is unfocused so it doesn't
# generate .meta files.

# Then, run the script in admin mode from a Python Console
# and delete the leftover files in the "Tools"" directory: 
# EOSUpdate.zip, the unarchived contents, and the old files
# which have been placed into the "OldSDKFiles" folder

import zipfile
import shutil
import os
from pathlib import Path

sdkFolder = "../Runtime/EOSSDK"
sdkZipFileName = "EOSUpdate.zip"
sdkUnzippedFolderName = "EOSUpdateExtracted"
junkFolderName = "OldSDKFiles"

def unzip_file(zip_file_path, extract_to):
	with zipfile.ZipFile(zip_file_path, 'r') as zip_ref:
		zip_ref.extractall(extract_to)
	print(f"Extracted {zip_file_path} to {extract_to}")

def copy_file(source_file, destination_file):
	source = Path(source_file)
	dest = Path(destination_file)
	
	if(not source.exists()):
		print(f"Error: Source path {source_file} did not exist!")
		return
	if(not dest.exists()):
		print(f"Error: Dest path {destination_file} did not exist!")
		return
	
	try:
		shutil.copy2(source, dest)
		print(f"- Copied {source} to {dest}")
	except Exception as e:
		print(f"Error: Exception when copying file!")
		print(e)

def copy_tree(src, dst, symlinks=False, ignore=None):
	for item in os.listdir(src):
		s = os.path.join(src, item)
		d = os.path.join(dst, item)
		try:
			if os.path.isdir(s):
				shutil.copytree(s, d, symlinks, ignore)
			else:
				shutil.copy2(s, d)
		except Exception as e:
			print(f"Error: Exception when copying folder!")
			print(e)
	
	# Done copying tree
	print(f"- Copied {src} to {dst}")

def move_file(src, dst):
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

def move_file_to_junk(src):
	sourcePath = Path(src)
	if(not sourcePath.exists()):
		print(f"No file {src} found to move to {junkFolderName}, skipping...")
		return

	junkPath = Path(junkFolderName)
	if(not junkPath.exists()):
		os.makedirs(junkPath)

	if(not junkPath.exists()):
		print(f"Error: JunkPath was not created correctly")
		return

	move_file(src, os.path.join(junkPath, src))

def move_unity_file_to_junk(src):
	move_file_to_junk(src)
	move_file_to_junk(src + ".meta")

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
	# so they can be deleted easily. This is safer than trying to "rm"
	# from Python just in case something goes wrong.

	pathToSDKParent = sdkFolder + "/SDK"
	pathToThirdPartyNotices = sdkFolder + "/ThirdPartyNotices"

	move_unity_file_to_junk(pathToSDKParent)
	move_unity_file_to_junk(pathToThirdPartyNotices)

	print("Success")
	return

	# Now copy the relevant files
	
	copy_file(pathToUnzipFolderString + "/ThirdPartyNotices/ThirdPartySoftwareNotice.txt", sdkFolder + "/ThirdPartyNotices/ThirdPartySoftwareNotice.txt")
	copy_file(pathToUnzipFolderString + "/SDK/Bin/EOSSDK-Win32-Shipping.dll", sdkFolder + "/SDK/Plugins/EOSSDK-Win32-Shipping.dll")
	copy_file(pathToUnzipFolderString + "/SDK/Bin/EOSSDK-Win64-Shipping.dll", sdkFolder + "/SDK/Plugins/EOSSDK-Win64-Shipping.dll")
	copy_file(pathToUnzipFolderString + "/SDK/Bin/libEOSSDK-Linux-Shipping.so", sdkFolder + "/SDK/Plugins/libEOSSDK-Linux-Shipping.so")
	copy_file(pathToUnzipFolderString + "/SDK/Bin/libEOSSDK-Mac-Shipping.dylib", sdkFolder + "/SDK/Plugins/libEOSSDK-Mac-Shipping.dylib")
	
	#TODO: This doesn't seem to work right...
	#Help wanted: how to copy entire folders correctly?
	#For now, please copy these manually from the file system
	#TODO: ALSO! Sometimes files can get deleted in newer versions
	#So really what needs to happen is to delete the "Source" folder first
	#and then copy in the new contents
	copy_tree(pathToUnzipFolderString + "/SDK/Bin/x86/", sdkFolder + "/SDK/Plugins/x86/")
	copy_tree(pathToUnzipFolderString + "/SDK/Bin/x64/", sdkFolder + "/SDK/Plugins/x64/")
	copy_tree(pathToUnzipFolderString + "/SDK/Source/", sdkFolder + "/SDK/Source/")
	
	print(f"Update complete!")
	

_main()

