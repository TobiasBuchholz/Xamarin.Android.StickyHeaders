﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;

namespace Xamarin.Android.StickyHeaders
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            // InitWithSectionAdapter();
            InitWithSectionIndexAdapter();
        }

        private void InitWithSectionAdapter()
        {
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var dividerDecoration = new DividerItemDecoration(this, DividerItemDecoration.Vertical);
            recyclerView.AddItemDecoration(dividerDecoration);

            var adapter = new SectionAdapter();
            recyclerView.SetAdapter(adapter);

            var stickyHeaderDecoration = new StickyHeaderItemDecoration(adapter);
            stickyHeaderDecoration.AttachToRecyclerView(recyclerView);

            var items = new List<ISection>();
            var section = 0;
            for(var i = 0; i < 28; i++) {
                if(i < 12) {
                    if(i % 4 == 0) {
                        section = i;
                        items.Add(new SectionHeader(section));
                    } else {
                        items.Add(new SectionItem(section));
                    }
                } else {
                    if(i % 8 == 0) {
                        section = i;
                        items.Add(new SectionHeader(section));
                    } else {
                        items.Add(new SectionItem(section));
                    }
                }
            }

            adapter.Items = items;
            adapter.NotifyDataSetChanged();
        }
        
        private void InitWithSectionIndexAdapter()
        {
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var dividerDecoration = new DividerItemDecoration(this, DividerItemDecoration.Vertical);
            recyclerView.AddItemDecoration(dividerDecoration);

            var adapter = new SectionIndexAdapter<SimpleItem>();
            recyclerView.SetAdapter(adapter);

            var stickyHeaderDecoration = new StickyHeaderItemDecoration(adapter);
            stickyHeaderDecoration.AttachToRecyclerView(recyclerView);

            var items = Enumerable.Range(0, 15).Select(x => new SimpleItem($"Item #{x}")).ToList();
            var sectionIndexes = new[] { 0, 4, 6 };
            adapter.Items = items;
            adapter.SectionIndexes = sectionIndexes;
            adapter.NotifyDataSetChanged();
        }
    }
}