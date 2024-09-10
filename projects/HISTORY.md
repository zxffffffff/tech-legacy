# history

记录历史操作（执行命令）

## 2024-09-09

### 创建 vite + react + ts 项目

[脚手架 create-vite](https://github.com/vitejs/vite/tree/main/packages/create-vite)

```sh
# npm 7+，需要添加额外的 --：
npm create vite@latest tech-legacy-web -- --template react-ts
```

### 引入第三方依赖

[3D 引擎 react-three-fiber](https://r3f.docs.pmnd.rs/getting-started/introduction)
[物理引擎 react-three-cannon](https://github.com/pmndrs/use-cannon/blob/master/packages/react-three-cannon/README.md)

```sh
# three.js
npm install three @types/three @react-three/fiber

# cannon-es
npm install @react-three/cannon
```

### 引入第三方组件

[端侧路由 React Router](https://reactrouter.com/en/main/start/overview)
[本地存储 localForage](https://localforage.github.io/localForage/)

```sh
npm install react-router-dom localforage
```
