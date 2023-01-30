import { useState, useEffect } from "react"
import { getBikes } from "../bikeManager.js";
import BikeCard from "./BikeCard"

export default function BikeList({setDetailsBikeId}) {
    const [bikes, setBikes] = useState([])

    useEffect(() => {
        getBikes().then(bikes =>
        setBikes(bikes));
    }, []);
    return (
        <>
        <h2>Bikes</h2>
        <div className="bike-list">
        {bikes.map((b) => (
            <BikeCard key={b.id} bike={b} setDetailsBikeId={setDetailsBikeId} />
        ))}
        </div>
        </>
    );
}