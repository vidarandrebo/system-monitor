build: build-frontend
	-rm -r bin
	dotnet publish src/WebAPI/ -c Release -o bin

build-frontend:
	-rm -r src/WebAPI/wwwroot/*
	cd src/WebUI/ && npm run build
	cp -r src/WebUI/dist/* src/WebAPI/wwwroot/

clean:
	-rm -rf bin
	find ./src/Domain -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/Application -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/Infrastructure -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/ConsoleInterface -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/WebAPI -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/WebUI -type d \( -name "dist" \) -exec rm -rf {} +

install: uninstall
	cp cmd/system-monitor /usr/local/bin/system-monitor
	- mkdir /var/log/system-monitor
	chmod -R 777 /var/log/system-monitor
	cp -R bin /usr/local/share/system-monitor
	
uninstall:
	-rm /usr/local/bin/system-monitor
	-rm -r /usr/local/share/system-monitor
	-rm -r /var/log/system-monitor
