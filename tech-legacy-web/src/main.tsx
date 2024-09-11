import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
// import './index.css'
import ErrorPage from "./error-page.tsx";
import App from './App.tsx'
import ExampleApp from './example/exampleApp.tsx';
import Blog from './example/blog/Blog.tsx';

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <ErrorPage />,
  },
  {
    path: "/example",
    element: <ExampleApp />,
  },
  {
    path: "/example/blog",
    element: <Blog />,
  },
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
)
