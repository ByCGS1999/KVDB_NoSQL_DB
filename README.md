# KVDB_NoSQL_DB
My own implementation of a Key-Value based Database, it uses a TCP Socket for Client <-> Server Communication.

Keys are unique, this means that two registries MUST NOT HAVE the same key, this is limited in the code and it wont allow you create two registries with the same key.

File Format -> .kvdb (Key-Value DataBase).

About Packets.
Packets must be encoded with ASCII (unsafe / no encryption).
Actually applying a layer of encryption in top of the packets is easy, you shouldn't have any problem doing it.
Packet Chart:
As separator we will use the ascii character '|', i decided to use this character since its not a common character.
addReg [key] [value] - Adds a new value to the Registry.
Sample -> addReg|hello|world
  operation ^ key ^ value ^
deleteReg [key] - Removes a value to the Registry.
Sample -> deleteReg|hello
    operation ^   key ^ 
checkReg [key] - Checks if the key exists in the registry.
Sample -> checkReg|hello
  operation ^    key ^ 
