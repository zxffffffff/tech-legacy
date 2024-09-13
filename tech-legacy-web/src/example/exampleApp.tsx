import { useState } from 'react'
import reactLogo from '../assets/react.svg'
import viteLogo from '/vite.svg'
import styles from './exampleApp.module.css'
import CannonCubeHeap from './cannonCubeHeap'

export default function ExampleApp() {
  const [count, setCount] = useState(0)

  return (
    <div className={styles.root}>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className={styles.logo} alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className={`${styles.logo} ${styles['logo.react']}`} alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>

      <CannonCubeHeap />

      <div className={styles.card}>
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className={styles['read-the-docs']}>
        Click on the Vite and React logos to learn more
      </p>
    </div>
  )
}