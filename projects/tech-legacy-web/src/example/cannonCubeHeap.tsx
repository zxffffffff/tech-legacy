import type { PlaneProps, Triplet } from '@react-three/cannon'
import { Physics, useBox, usePlane, useSphere } from '@react-three/cannon'
import { Canvas, useFrame } from '@react-three/fiber'
import { useMemo, useRef, useState } from 'react'
import type { InstancedMesh, Mesh } from 'three'
import { Color } from 'three'

const niceColors = ['#99b898', '#fecea8', '#ff847c', '#e84a5f', '#2a363b'];

function Plane(props: PlaneProps) {
    const [ref] = usePlane(() => ({ ...props }), useRef<Mesh>(null))
    return (
        <mesh ref={ref} receiveShadow>
            <planeGeometry args={[10, 10]} />
            <shadowMaterial color="#171717" />
        </mesh>
    )
}

type InstancedGeometryProps = {
    colors: Float32Array
    number: number
    size: number
}

const Spheres = ({ colors, number, size }: InstancedGeometryProps) => {
    const [ref, { at }] = useSphere(
        () => ({
            args: [size],
            mass: 1,
            position: [Math.random() - 0.5, Math.random() * 2, Math.random() - 0.5],
        }),
        useRef<InstancedMesh>(null),
    )
    useFrame(() => at(Math.floor(Math.random() * number)).position.set(0, Math.random() * 2, 0))
    return (
        <instancedMesh receiveShadow castShadow ref={ref} args={[undefined, undefined, number]}>
            <sphereGeometry args={[size, 48]}>
                <instancedBufferAttribute attach="attributes-color" args={[colors, 3]} />
            </sphereGeometry>
            <meshLambertMaterial vertexColors />
        </instancedMesh>
    )
}

const Boxes = ({ colors, number, size }: InstancedGeometryProps) => {
    const args: Triplet = [size, size, size]
    const [ref, { at }] = useBox(
        () => ({
            args,
            mass: 1,
            position: [Math.random() - 0.5, Math.random() * 2, Math.random() - 0.5],
        }),
        useRef<InstancedMesh>(null),
    )
    useFrame(() => at(Math.floor(Math.random() * number)).position.set(0, Math.random() * 2, 0))
    return (
        <instancedMesh receiveShadow castShadow ref={ref} args={[undefined, undefined, number]}>
            <boxGeometry args={args}>
                <instancedBufferAttribute attach="attributes-color" args={[colors, 3]} />
            </boxGeometry>
            <meshLambertMaterial vertexColors />
        </instancedMesh>
    )
}

const instancedGeometry = {
    box: Boxes,
    sphere: Spheres,
}

export default function CannonCubeHeap() {
    const [geometry, setGeometry] = useState<'box' | 'sphere'>('box')
    const [number] = useState(200)
    const [size] = useState(0.1)

    const colors = useMemo(() => {
        const array = new Float32Array(number * 3)
        const color = new Color()
        for (let i = 0; i < number; i++)
            color
                .set(niceColors[Math.floor(Math.random() * 5)])
                .convertSRGBToLinear()
                .toArray(array, i * 3)
        return array
    }, [number])

    const InstancedGeometry = instancedGeometry[geometry]

    return (
        <Canvas
            camera={{ fov: 50, position: [-1, 1, 2.5] }}
            onCreated={({ scene }) => (scene.background = new Color('lightblue'))}
            onPointerMissed={() => setGeometry((geometry) => (geometry === 'box' ? 'sphere' : 'box'))}
            shadows
        >
            <hemisphereLight intensity={0.35 * Math.PI} />
            <spotLight
                angle={0.3}
                castShadow
                decay={0}
                intensity={2 * Math.PI}
                penumbra={1}
                position={[10, 10, 10]}
            />
            <Physics broadphase="SAP">
                <Plane rotation={[-Math.PI / 2, 0, 0]} />
                <InstancedGeometry {...{ colors, number, size }} />
            </Physics>
        </Canvas>
    )
}