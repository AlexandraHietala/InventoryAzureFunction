using Microsoft.SqlServer.Server;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.System;
using System.Collections.Generic;
using System.Drawing;

namespace InventoryFunction.Validators.LightValidators
{
    public interface IItemLightValidator
    {
        string ValidateItemId(int id);
        string ValidateCollectionId(int id);
        string ValidateAddItem(Item item);
        string ValidateUpdateItem(Item item);
    }

    public class ItemLightValidator : IItemLightValidator
    {
        public string ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200200020, Message = "Item object is invalid." });

            if (item != null && item.CollectionId != null && item.CollectionId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200021, Message = "Collection Id is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Status))
                failureList.Add(new ValidationFailure() { Code = 200200022, Message = "Status is required." });

            if (item != null && string.IsNullOrEmpty(item.Type))
                failureList.Add(new ValidationFailure() { Code = 200200023, Message = "Type is required." });

            if (item != null && string.IsNullOrEmpty(item.Format))
                failureList.Add(new ValidationFailure() { Code = 200200024, Message = "Format is required." });

            if (item != null && string.IsNullOrEmpty(item.Size))
                failureList.Add(new ValidationFailure() { Code = 200200025, Message = "Size is required." });

            if (item != null && item.BrandId != null && item.BrandId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200026, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200027, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && !int.TryParse(item.BrandId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200028, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200029, Message = "Brand is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200030, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200031, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && !int.TryParse(item.SeriesId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200032, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200033, Message = "Series is invalid." });

            if (item != null && item.Status != null && (item.Status != Statuses.Pending && item.Status != Statuses.Wishlist && item.Status != Statuses.Owned && item.Status != Statuses.NotOwned))
                failureList.Add(new ValidationFailure() { Code = 200200034, Message = "Status selection is invalid." });

            if (item != null && item.Type != null && (item.Type != Types.Set && item.Type != Types.SoldSeparately && item.Type != Types.Blind))
                failureList.Add(new ValidationFailure() { Code = 200200035, Message = "Type selection is invalid." });
           
            if (item != null && item.Format != null && (item.Format != Formats.Plush && item.Format != Formats.Figure && item.Format != Formats.Keychain && item.Format != Formats.Other))
                failureList.Add(new ValidationFailure() { Code = 200200036, Message = "Format selection is invalid." });

            if (item != null && item.Size != null && (item.Size != Sizes.Irregular && item.Size != Sizes.Large && item.Size != Sizes.Regular && item.Size != Sizes.Mini))
                failureList.Add(new ValidationFailure() { Code = 200200037, Message = "Size selection is invalid." });

            if (item != null && item.Name != null && item.Name.Length > 100)
                failureList.Add(new ValidationFailure() { Code = 200200038, Message = "Name is too long." });

            if (item != null && item.Description != null && item.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200039, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200200040, Message = "Item object is invalid." });

            if (item != null && item.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200041, Message = "Item Id is invalid." });

            if (item != null && item.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200042, Message = "Item Id is invalid." });

            if (item != null && item.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200043, Message = "Item Id is invalid." });

            if (item != null && !int.TryParse(item.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200044, Message = "Item Id is invalid." });

            if (item != null && item.CollectionId != null && item.CollectionId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200045, Message = "Collection Id is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Status))
                failureList.Add(new ValidationFailure() { Code = 200200046, Message = "Status is required." });

            if (item != null && string.IsNullOrEmpty(item.Type))
                failureList.Add(new ValidationFailure() { Code = 200200047, Message = "Type is required." });

            if (item != null && string.IsNullOrEmpty(item.Format))
                failureList.Add(new ValidationFailure() { Code = 200200048, Message = "Format is required." });

            if (item != null && string.IsNullOrEmpty(item.Size))
                failureList.Add(new ValidationFailure() { Code = 200200049, Message = "Size is required." });

            if (item != null && item.BrandId != null && item.BrandId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200050, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200051, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && !int.TryParse(item.BrandId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200052, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200053, Message = "Brand is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200054, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200055, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && !int.TryParse(item.SeriesId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200056, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200057, Message = "Series is invalid." });

            if (item != null && item.Status != null && (item.Status != Statuses.Pending && item.Status != Statuses.Wishlist && item.Status != Statuses.Owned && item.Status != Statuses.NotOwned))
                failureList.Add(new ValidationFailure() { Code = 200200058, Message = "Status selection is invalid." });

            if (item != null && item.Type != null && (item.Type != Types.Set && item.Type != Types.SoldSeparately && item.Type != Types.Blind))
                failureList.Add(new ValidationFailure() { Code = 200200059, Message = "Type selection is invalid." });

            if (item != null && item.Format != null && (item.Format != Formats.Plush && item.Format != Formats.Figure && item.Format != Formats.Keychain && item.Format != Formats.Other))
                failureList.Add(new ValidationFailure() { Code = 200200060, Message = "Format selection is invalid." });

            if (item != null && item.Size != null && (item.Size != Sizes.Irregular && item.Size != Sizes.Large && item.Size != Sizes.Regular && item.Size != Sizes.Mini))
                failureList.Add(new ValidationFailure() { Code = 200200061, Message = "Size selection is invalid." });

            if (item != null && item.Name != null && item.Name.Length > 100)
                failureList.Add(new ValidationFailure() { Code = 200200062, Message = "Name is too long." });

            if (item != null && item.Description != null && item.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200063, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateItemId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200064, Message = "Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200065, Message = "Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200066, Message = "Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateCollectionId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200067, Message = "Collection Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200068, Message = "Collection Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200069, Message = "Collection Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
