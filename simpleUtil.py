
from distutils.file_util import copy_file
import os
path='D:/Steam/steamapps/common/worldbox/Mods/Cultivation-Way/GameResources/actors'
dir=os.listdir(path)
k=0
for i in dir:
    #copy_file("sprites.json",os.path.join(path,i))
    if(not i[0].isupper()):
        continue
    print(i)
    try:
        os.rename(os.path.join(path,i),os.path.join(path,"t_"+i))
    except:
        continue
    k+=1