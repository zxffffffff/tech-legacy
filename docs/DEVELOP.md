# develop

## 先说结论

- 使用 `Godot 4` 开发
- 使用 `GDExtension/C++` 开发基础组件，使用 `GDScript` 开发游戏逻辑（我不喜欢 .NET/C#）
- 目前 4.3 导出 `Web` 平台仍然存在问题，希望可以解决，否则降级 3.x

## R&D

### 跨平台 > 开发效率(调试) > 体验(性能)

1. Mobile (Android, iOS, iPad) 🌟🌟🌟🌟🌟
2. PC (Windows, macOS, Linux) 🌟🌟🌟
3. XR (Meta Quest, PICO, Apple Vision Pro) 🌟

### 可选 GUI 方案

- [React Native (JS)](https://github.com/facebook/react-native.git), [Expo](https://github.com/expo/expo.git)
- [Flutter (Dart)](https://github.com/flutter/flutter.git)
- [.NET MAUI (C#)](https://github.com/dotnet/maui.git)
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
- Microsoft Havok (C++), [Havok Physics for Web (WASM)](https://github.com/BabylonJS/havok.git)
- [NVIDIA PhysX (C++, WASM)](https://github.com/NVIDIA-Omniverse/PhysX.git)
- [Bullet (C++)](https://github.com/bulletphysics/bullet3.git), [ammo.js (WASM)](https://github.com/kripken/ammo.js.git)
- [Jolt (C++)](https://github.com/jrouwe/JoltPhysics.git), [JoltPhysics.js (WASM)](https://github.com/jrouwe/JoltPhysics.js.git)
- [cannon.js](https://github.com/pmndrs/cannon-es.git)

## 资料

- [RN - New Architecture in Beta 0.74](https://reactnative.dev/docs/next/the-new-architecture/landing-page)
- [Flutter - Roadmap](https://github.com/flutter/flutter/blob/master/docs/roadmap/Roadmap.md)
- [Unity - as library in 2019.3 (Android, iOS, Windows)](https://docs.unity3d.com/Manual/UnityasaLibrary.html)
- [UE - as library in 4.27 (Windows)](https://forums.unrealengine.com/t/ue-4-27-preview-ue-as-a-lib/484701)
- [Godot - as library in Milestone 4.4 (Android)](https://github.com/godotengine/godot/pull/90510)
- [Godot - Web Export in 4.3](https://godotengine.org/article/progress-report-web-export-in-4-3/)
- [Godot - GDExtension in 4.0](https://godotengine.org/article/introducing-gd-extensions/)
- [Babylon - Using Havok and the Havok Plugin](https://doc.babylonjs.com/features/featuresDeepDive/physics/havokPlugin)
