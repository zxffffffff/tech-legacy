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

### 像素美术配置

- 项目设置/渲染/纹理/默认纹理过滤：Nearest，像素放大不会变模糊

### 单例模式配置

- 项目设置/全局/自动加载：添加 C# 脚本和节点名称

### 3rd 依赖

- 在外部目录管理依赖项 `../tech-legacy-3rd`

```sh
# forked from neogeek/rhythm-game-utilities
git submodule add https://github.com/zxffffffff/rhythm-game-utilities.git rhythm-game-utilities
git submodule update --init --recursive
```
