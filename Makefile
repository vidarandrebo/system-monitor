run: build
	./bin/UserInterface

build:
	dotnet publish src/UserInterface/ -c Release -o bin

clean:
	rm -rf bin
	find ./src/Domain -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/Monitors -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/UserInterface -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +

install: build
	cp -R bin/UserInterface /usr/local/bin/system-monitor
	mkdir /var/log/system-monitor
	chmod -R 777 /var/log/system-monitor
	make clean
	
uninstall:
	rm -r /usr/local/bin/system-monitor
	rm -r /var/log/system-monitor