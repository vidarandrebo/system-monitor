run: build
	./bin/ConsoleInterface

build:
	dotnet publish src/ConsoleInterface/ -c Release -o bin

clean:
	rm -rf bin
	find ./src/Domain -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/Application -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/Infrastructure -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/ConsoleInterface -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/WebAPI -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +

install: build
	cp -R bin/ConsoleInterface /usr/local/bin/system-monitor
	mkdir /var/log/system-monitor
	chmod -R 777 /var/log/system-monitor
	make clean
	
uninstall:
	rm -r /usr/local/bin/system-monitor
	rm -r /var/log/system-monitor