db = db.getSiblingDB("rentingfleet");

const vehicles = [
  { _id: UUID("a1111111-1111-4111-8111-111111111111"), Brand: "Toyota", Model: "Corolla", Year: 2024 },
  { _id: UUID("a2222222-2222-4222-8222-222222222222"), Brand: "Ford", Model: "Focus", Year: 2023 },
  { _id: UUID("a3333333-3333-4333-8333-333333333333"), Brand: "Volkswagen", Model: "Golf", Year: 2022 },
  { _id: UUID("a4444444-4444-4444-8444-444444444444"), Brand: "Renault", Model: "Clio", Year: 2025 },
  { _id: UUID("a5555555-5555-4555-8555-555555555555"), Brand: "Peugeot", Model: "308", Year: 2024 }
];

vehicles.forEach(vehicle => {
  db.Vehicles.updateOne(
    { _id: vehicle._id },
    { $setOnInsert: vehicle },
    { upsert: true }
  );
});
