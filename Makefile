run: build
	./bin/UserInterface

build:
	dotnet publish src/UserInterface/ -c Release -o bin

clean:
	rm -rf bin
	find ./src/Domain -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/Monitors -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
	find ./src/UserInterface -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} +
