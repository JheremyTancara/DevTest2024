import { BrowserRouter as Router, Routes, Route } from "react-router-dom"
import ProductsComponent from "./pages/AppComponent"
function App() {
  return (
    <Router>
    <Routes>
      <Route path="/" element={<ProductsComponent />} />
    </Routes>
  </Router>
  )
}

export default App