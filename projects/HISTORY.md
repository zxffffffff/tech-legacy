# 2024-09-02

创建 react native expo 项目

```sh
npx create-expo-app@latest

What is your app named? … tech-legacy

Your project is ready!

To run your project, navigate to the directory and run one of the following npm commands.

- cd tech-legacy
- npm run android
- npm run ios
- npm run web
```

macOS 处理报错
> Error: EMFILE: too many open files, watch
> at FSEvent.FSWatcher._handle.onchange (node:internal/fs/watchers:207:21)

```sh
brew update
brew install watchman

rm -rf node_modules
npm install
```

添加 three.js 和 react-three-fiber

```sh
# 参考 expo 模块安装
npx expo install expo-gl

npm install three @react-three/fiber @react-three/drei
```

遇到 bug

- [嵌套窗口cube消失](https://github.com/pmndrs/react-three-fiber/issues/3332)