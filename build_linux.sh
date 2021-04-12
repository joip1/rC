echo Building rC...
sudo csc src/*.cs
sudo mv ./Program.exe ./rC.exe
sudo cp ./rC.exe /usr/bin/rC
