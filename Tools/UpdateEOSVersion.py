
#WARNING: Experimental! Don't use this yet!

#HOW TO USE THIS:

#Move the EOS SDK Download to the "Tools" directory
#and rename it to EOSUpdate.zip

#Then, run the script and delete the leftover files in the 
#Tools directory: EOSUpdate.zip and the unarchived contents

#You must run in admin mode from Windows Terminal/CMD

import zipfile
import shutil
import os
from pathlib import Path

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
	
	#done copying tree
	print(f"- Copied {src} to {dst}")

def _main():
	print(f"Starting EOS Updater...")
	print(f"Be sure to move the SDK zip to this directory & name it EOSUpdate.zip")
	
	# Unzip the file
	zip_file_path = 'EOSUpdate.zip'
	zip_file = Path(zip_file_path)
	if(not zip_file.is_file()):
		#zip file doesn't exist
		print(f"Error: No EOSUpdate.zip found in same directory")
		return
	
	extract_to = 'EOSUpdateExtracted'
	unzip_file(zip_file_path, extract_to)
	
	extracted_folder = Path(extract_to)
	if(not extracted_folder.is_dir()):
		#folder doesn't exist
		print(f"Error: No EOSUpdateExtracted found in same directory")
		return
	
	#Now copy the relevant files
	
	sdkfolder = "../Runtime/EOSSDK"
	copy_file(extract_to + "/ThirdPartyNotices/ThirdPartySoftwareNotice.txt", sdkfolder + "/ThirdPartyNotices/ThirdPartySoftwareNotice.txt")
	copy_file(extract_to + "/SDK/Bin/EOSSDK-Win32-Shipping.dll", sdkfolder + "/SDK/Plugins/EOSSDK-Win32-Shipping.dll")
	copy_file(extract_to + "/SDK/Bin/EOSSDK-Win64-Shipping.dll", sdkfolder + "/SDK/Plugins/EOSSDK-Win64-Shipping.dll")
	copy_file(extract_to + "/SDK/Bin/libEOSSDK-Linux-Shipping.so", sdkfolder + "/SDK/Plugins/libEOSSDK-Linux-Shipping.so")
	copy_file(extract_to + "/SDK/Bin/libEOSSDK-Mac-Shipping.dylib", sdkfolder + "/SDK/Plugins/libEOSSDK-Mac-Shipping.dylib")
	
	#TODO: This doesn't seem to work right...
	#Helped wanted: how to copy entire folders correctly?
	#For now, please copy these manually from the file system
	#TODO: ALSO! Sometimes files can get deleted in newer versions
	#So really what needs to happen is to delete the "Source" folder first
	#and then copy in the new contents
	copy_tree(extract_to + "/SDK/Bin/x86/", sdkfolder + "/SDK/Plugins/x86/")
	copy_tree(extract_to + "/SDK/Bin/x64/", sdkfolder + "/SDK/Plugins/x64/")
	copy_tree(extract_to + "/SDK/Source/", sdkfolder + "/SDK/Source/")
	
	print(f"Update complete!")
	

_main()

