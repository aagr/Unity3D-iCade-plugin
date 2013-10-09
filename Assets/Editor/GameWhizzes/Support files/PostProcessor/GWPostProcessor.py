from mod_pbxproj import XcodeProject
import os, sys, re, json, subprocess, fnmatch

#find files of a given extension in a give path
def find_files(path, extension='.txt'):
  if not os.path.isdir(path) or not extension:
    return []

  extension = extension.lower()

  file_list = os.listdir(path)
  # make sure they get processed in alpha order
  file_list.sort()

  return [os.path.join(path, f) for f in file_list if os.path.splitext(f)[1].lower() == extension]



#xcode project
project = XcodeProject.Load(sys.argv[1]+'/Unity-iPhone.xcodeproj/project.pbxproj')

#directory where .projmods are
package_path=sys.argv[2]


#find all .projmods

#mods = find_files(package_path, '.projmods')
#mods.extend(find_files(plugin_path, '.projmods'))

mods = []
for root, dirnames, filenames in os.walk(package_path):
    for filename in fnmatch.filter(filenames, '*.projmods'):
        mods.append(os.path.join(root, filename))

#apply each .projmod to the project
for mod in mods:
  with open(mod) as f:
    json_dict = json.loads(f.read())

  project.apply_mods(json_dict, package_path)


#if the project was modified, save a backup and save
if project.modified:
    project.backup()
    project.saveFormat3_2()