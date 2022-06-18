
from distutils.file_util import copy_file
import os
path='D:/Steam/steamapps/common/worldbox/Mods/Cultivation-Way/GameResources/actors/races/Wu'
dir=os.listdir(path)
k=0
for i in dir:
    copy_file("sprites.json",os.path.join(path,i))
    print(i)
    #os.rename(os.path.join(path,i),os.path.join(path,"t_"+i))
    k+=1