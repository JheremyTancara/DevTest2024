import React, { useEffect, useState } from "react";
import { PollOption, RegisterPollOptionsDTO } from "../service/types/PollOptionTypes";
import PullOptionsService from "../service/PullOptionService";

const apiService = new PullOptionsService("http://localhost:5142/api/v1");

const ProductsComponent: React.FC = () => {
  const [products, setProducts] = useState<PollOption[]>([]);
  const [newProduct, setNewProduct] = useState<RegisterPollOptionsDTO>({
    name: "",
    votes: 0
  });

  const fetchProducts = async () => {
    try {
      const data = await apiService.getProducts();
      setProducts(data);
    } catch (error) {
      console.error("Error to get pulloptions:", error);
    }
  };

  const handleCreateProduct = async () => {
    try {
      const createdProduct = await apiService.createProduct(newProduct);
      setProducts([...products, createdProduct]);
      setNewProduct({ name: "", votes: 0});
    } catch (error) {
      console.error("Error to create pulloption:", error);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  return (
    <div className="justify-container">
      <div className="p-4">
        <h2 className="text-xl font-bold mb-4">PollOptions</h2>
        <div className="overflow-y-auto max-h-[500px]">
          <table className="min-w-full table-auto border-collapse">
            <thead>
              <tr className="bg-gray-100">
                <th className="px-4 py-2 border-b">ID PollOption</th>
                <th className="px-4 py-2 border-b">Name</th>
                <th className="px-4 py-2 border-b">Votes</th>
                <th className="px-4 py-2 border-b">CreatedAt</th>
              </tr>
            </thead>
            <tbody>
              {products.slice(0, 20).map((product) => (
                <tr key={product.pollOptionID} className="hover:bg-gray-50">
                  <td className="px-4 py-2 border-b">{product.pollOptionID}</td>
                  <td className="px-4 py-2 border-b">{product.name}</td>
                  <td className="px-4 py-2 border-b">${product.votes}</td>
                  <td className="px-4 py-2 border-b">${product.createdAt}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <h3 className="text-lg font-semibold mb-2 mt-6">
          Create a new pull option
        </h3>
        <div className="space-y-4">
          <input
            type="text"
            value={newProduct.name}
            onChange={(e) =>
              setNewProduct({ ...newProduct, name: e.target.value })
            }
            placeholder="Name Poll Option"
            className="p-2 border rounded w-full"
          />
          <input
            type="number"
            value={newProduct.votes}
            onChange={(e) =>
              setNewProduct({
                ...newProduct,
                votes: parseFloat(e.target.value),
              })
            }
            placeholder="Quantity of Votes"
            className="p-2 border rounded w-full"
          />
          <button
            onClick={handleCreateProduct}
            className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
          >
            Create Poll Option
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductsComponent;
