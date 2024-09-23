# history

记录历史操作（配置、执行命令）

## 2024-09-23

### 下载 .NET 最新版本 (8.0+)

- <https://dotnet.microsoft.com/zh-cn/download>

### 配置 .NET 环境变量

```sh
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools

dotnet --info
sudo dotnet workload update
```

### VS Code 安装 C# 扩展

项目配置 “Run and Debug” 用于调试 C# 代码

- .vscode/launch.json

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Play",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            // /Applications/Godot_mono.app/Contents/MacOS/Godot
            // C:/Program Files/Godot/Godot.exe
            "program": "/Applications/Godot_mono.app/Contents/MacOS/Godot",
            "args": [],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
        }
    ]
}
```

- .vscode/tasks.json

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build"
            ],
            "problemMatcher": "$msCompile",
            "presentation": {
                "echo": true,
                "reveal": "silent",
                "focus": false,
                "panel": "shared",
                "showReuseMessage": true,
                "clear": false
            }
        }
    ]
}
```
