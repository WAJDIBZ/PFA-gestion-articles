import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { RouteProtegee } from "./components/RouteProtegee";
import Layout from "./components/Layout";
import Landing from "./pages/Landing";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Articles from "./pages/Articles";
import ArticleDetail from "./pages/ArticleDetail";
import Catalogue from "./pages/Catalogue";
import Depots from "./pages/Depots";
import Stock from "./pages/Stock";
import Mouvements from "./pages/Mouvements";
import Fournisseurs from "./pages/Fournisseurs";
import Utilisateurs from "./pages/Utilisateurs";

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/" element={<Landing />} />
          <Route path="/login" element={<Login />} />

          <Route element={<RouteProtegee />}>
            <Route element={<Layout />}>
              <Route path="/dashboard" element={<Dashboard />} />
              <Route path="/articles" element={<Articles />} />
              <Route path="/articles/:id" element={<ArticleDetail />} />
              <Route path="/catalogue" element={<Catalogue />} />
              <Route path="/depots" element={<Depots />} />
              <Route path="/stock" element={<Stock />} />
              <Route path="/mouvements" element={<Mouvements />} />
              <Route path="/fournisseurs" element={<Fournisseurs />} />

              <Route
                element={<RouteProtegee rolesAutorises={["Administrateur"]} />}
              >
                <Route path="/utilisateurs" element={<Utilisateurs />} />
              </Route>
            </Route>
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
