# develop

## 结论

### 阶段一: 基于 Web 跨平台

使用 WebGL / WebXR 开发跨平台应用，优先浏览器访问

- 使用 3D 引擎，例如 three.js
- 集成物理引擎，例如 Bullet

## 过程

### 跨平台 > 开发效率(调试) > 体验(性能)

1. Mobile (Android, iOS, iPad) 🌟🌟🌟🌟🌟
2. PC (Windows, macOS, Linux) 🌟🌟🌟
3. XR (Meta Quest, PICO, Apple Vision Pro) 🌟

### 可选 GUI 方案

- [React Native (JS)](https://github.com/facebook/react-native.git), [Expo](https://github.com/expo/expo.git)
- [Flutter (Dart)](https://github.com/flutter/flutter.git)
- [MAUI (C#)](https://github.com/dotnet/maui.git)
- [Taro (JS) 小程序](https://github.com/NervJS/taro.git)
- Qt (C++) 商业收费
- [Electron (JS)](https://github.com/electron/electron.git)

### 可选 3D 方案

- Unity (C#, WASM) 商业收费
- Unreal Engine (Blueprint/C++) 商业收费
- [Godot (GDScript)](https://github.com/godotengine/godot.git), [GDExtension (C++, WASM)](https://github.com/godotengine/godot-cpp.git)
- [three.js](https://github.com/mrdoob/three.js.git), [react-three-fiber](https://github.com/pmndrs/react-three-fiber.git)
- [Babylon.js](https://github.com/BabylonJS/Babylon.js.git)
- [Filament (C++, WASM)](https://github.com/google/filament), [react-native-filament (JS)](https://github.com/margelo/react-native-filament.git)
- Havok
- [PhysX](https://github.com/NVIDIA-Omniverse/PhysX.git)
- [Bullet](https://github.com/bulletphysics/bullet3.git)
- [Jolt](https://github.com/jrouwe/JoltPhysics.git)

## 资料

- [RN - New Architecture in Beta 0.74](https://reactnative.dev/docs/next/the-new-architecture/landing-page)
- [Flutter - Roadmap](https://github.com/flutter/flutter/blob/master/docs/roadmap/Roadmap.md)
- [Unity - as library in 2019.3 (Android, iOS, Windows)](https://docs.unity3d.com/Manual/UnityasaLibrary.html)
- [UE - as library in 4.27 (Windows)](https://forums.unrealengine.com/t/ue-4-27-preview-ue-as-a-lib/484701)
- [Godot - as library in Milestone 4.4 (Android)](https://github.com/godotengine/godot/pull/90510)
- [Godot - Web Export in 4.3](https://godotengine.org/article/progress-report-web-export-in-4-3/)
- [Godot - GDExtension in 4.0](https://godotengine.org/article/introducing-gd-extensions/)
- [Babylon - Using Havok and the Havok Plugin](https://doc.babylonjs.com/features/featuresDeepDive/physics/havokPlugin)
