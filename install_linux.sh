echo "Building rC..."
sudo csc src/*.cs
echo "Built!"
echo "Installing..."
#sudo mv ./Program.exe ./rC.exe

sudo cp ./rC.exe /usr/bin/rC
#mv ./rC.exe ./rC
echo "If you saw no errors then congratulations, rC was Installed!"
