
import os
path='D:/Steam/steamapps/common/worldbox/Mods/Cultivation-Way/GameResources/actors'
dir=os.listdir(path)
k=0
for i in dir:
    os.rename(os.path.join(path,i),os.path.join(path,"t_"+i))
    k+=1